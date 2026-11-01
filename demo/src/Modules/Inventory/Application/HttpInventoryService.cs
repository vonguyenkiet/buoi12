using System.Net.Http.Json;
using Inventory.Contracts;
using Inventory.Contracts.DTOs;
using Microsoft.Extensions.Logging;

namespace Modules.Inventory.Application;

/// <summary>
/// HTTP REST Adapter cho IInventoryService:
/// Đóng vai trò là cầu nối giao tiếp mạng (Network Seam).
/// Ở giai đoạn Modular Monolith (P0), adapter này gọi qua loopback REST API endpoints.
/// Khi Inventory được bóc tách thành Microservice độc lập (P1), OrderService hoàn toàn không đổi một dòng code nào,
/// chỉ cần cấu hình BaseAddress của HttpClient trỏ tới máy chủ Inventory Microservice mới!
/// </summary>
public class HttpInventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpInventoryService> _logger;

    public HttpInventoryService(HttpClient httpClient, ILogger<HttpInventoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ReserveStockResult> ReserveAsync(ReserveStockRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[INVENTORY-HTTP] [SYNC-REST] Gửi HTTP POST /internal/inventory/reserve (ProductId: {ProductId}, Qty: {Qty}, OrderId: {OrderId})...",
            request.ProductId, request.Quantity, request.OrderId);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/internal/inventory/reserve", request, cancellationToken);
            var result = await response.Content.ReadFromJsonAsync<ReserveStockResult>(cancellationToken: cancellationToken);

            if (result != null)
            {
                _logger.LogInformation(
                    "[INVENTORY-HTTP] [SYNC-REST] Nhận phản hồi HTTP {StatusCode}: Success={Success}, Msg='{Msg}'",
                    (int)response.StatusCode, result.Success, result.Message);
                return result;
            }

            return new ReserveStockResult(false, "Không thể đọc dữ liệu phản hồi từ Inventory REST API.", 0, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[INVENTORY-HTTP] [SYNC-REST] Lỗi kết nối HTTP REST tới Inventory Service: {Message}", ex.Message);
            return new ReserveStockResult(false, $"Lỗi giao tiếp HTTP: {ex.Message}", 0, 0);
        }
    }

    public async Task<bool> ReleaseAsync(ReleaseStockRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "[INVENTORY-HTTP] [SYNC-REST] Gửi HTTP POST /internal/inventory/release (ProductId: {ProductId}, Qty: {Qty})...",
            request.ProductId, request.Quantity);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/internal/inventory/release", request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[INVENTORY-HTTP] [SYNC-REST] Lỗi HTTP Release: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<StockDto?> GetStockAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StockDto>($"/internal/inventory/{productId}", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[INVENTORY-HTTP] [SYNC-REST] Lỗi HTTP GetStock: {Message}", ex.Message);
            return null;
        }
    }
}
