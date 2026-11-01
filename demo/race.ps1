# Script kiểm thử tranh chấp đồng thời (Race Condition / Concurrency Test)
# Kịch bản: Hai khách hàng cùng bấm mua chiếc "Laptop Gaming Cuối Cùng" (tồn kho ban đầu = 1)
# Kỳ vọng kết quả: Nhờ câu lệnh UPDATE nguyên tử (Anti-overselling), đúng 1 khách nhận 201 Created và đúng 1 khách nhận 409 Conflict.

$baseUrl = "http://localhost:5000"
$productId = "33333333-3333-3333-3333-333333333333"

Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host "  KIỂM CHỨNG CHỐNG BÁN VƯỢT TỒN (RACE CONDITION CONCURRENCY TEST)" -ForegroundColor Yellow
Write-Host "==========================================================================" -ForegroundColor Cyan

# 1. Reset trạng thái kho
Write-Host "`n[1] Đang reset kho hàng về trạng thái ban đầu..." -ForegroundColor Gray
Invoke-RestMethod -Method Post -Uri "$baseUrl/demo/reset" | Out-Null

$stockBefore = Invoke-RestMethod -Method Get -Uri "$baseUrl/api/inventory/$productId"
Write-Host " -> Tồn kho ban đầu của 'Laptop Gaming Cuối Cùng': $($stockBefore.availableQuantity) chiếc khả dụng." -ForegroundColor Green

# 2. Chuẩn bị 2 request đồng thời từ 2 khách hàng khác nhau
$body1 = @{
    customerId = [Guid]::NewGuid()
    items = @( @{ productId = $productId; quantity = 1 } )
} | ConvertTo-Json

$body2 = @{
    customerId = [Guid]::NewGuid()
    items = @( @{ productId = $productId; quantity = 1 } )
} | ConvertTo-Json

Write-Host "`n[2] Bắn đồng thời 2 yêu cầu đặt mua song song (Parallel Requests)..." -ForegroundColor Yellow

$task1 = [System.Threading.Tasks.Task]::Run({
    param($url, $json)
    try {
        $res = Invoke-WebRequest -Uri "$url/api/orders" -Method Post -Body $json -ContentType "application/json" -ErrorAction Stop
        return @{ StatusCode = [int]$res.StatusCode; Content = $res.Content }
    } catch {
        return @{ StatusCode = [int]$_.Exception.Response.StatusCode; Content = (New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())).ReadToEnd() }
    }
}.GetNewClosure(), @($baseUrl, $body1))

$task2 = [System.Threading.Tasks.Task]::Run({
    param($url, $json)
    try {
        $res = Invoke-WebRequest -Uri "$url/api/orders" -Method Post -Body $json -ContentType "application/json" -ErrorAction Stop
        return @{ StatusCode = [int]$res.StatusCode; Content = $res.Content }
    } catch {
        return @{ StatusCode = [int]$_.Exception.Response.StatusCode; Content = (New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())).ReadToEnd() }
    }
}.GetNewClosure(), @($baseUrl, $body2))

[System.Threading.Tasks.Task]::WaitAll($task1, $task2)

$res1 = $task1.Result
$res2 = $task2.Result

Write-Host "`n[3] Kết quả nhận được từ máy chủ:" -ForegroundColor Cyan
Write-Host " -> Khách hàng 1: HTTP $($res1.StatusCode) | $($res1.Content)"
Write-Host " -> Khách hàng 2: HTTP $($res2.StatusCode) | $($res2.Content)"

# 3. Đánh giá kết quả
$codes = @($res1.StatusCode, $res2.StatusCode)
$count201 = ($codes | Where-Object { $_ -eq 201 }).Count
$count409 = ($codes | Where-Object { $_ -eq 409 }).Count

$stockAfter = Invoke-RestMethod -Method Get -Uri "$baseUrl/api/inventory/$productId"
Write-Host "`n -> Tồn kho sau tranh chấp: Khả dụng = $($stockAfter.availableQuantity), Đang giữ = $($stockAfter.reservedQuantity), Đã bán = $($stockAfter.soldQuantity)" -ForegroundColor Gray

if ($count201 -eq 1 -and $count409 -eq 1 -and $stockAfter.availableQuantity -eq 0) {
    Write-Host "`n✔ XÁC NHẬN THÀNH CÔNG: Chống bán vượt tồn hoạt động 100% chuẩn xác! (Đúng 1x201 và 1x409)" -ForegroundColor Green
} else {
    Write-Host "`n❌ THẤT BẠI: Phát hiện bán vượt tồn hoặc lỗi xử lý đồng thời!" -ForegroundColor Red
}
Write-Host "==========================================================================" -ForegroundColor Cyan
