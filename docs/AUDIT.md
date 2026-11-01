# BÁO CÁO KIỂM TOÁN HIỆN TRẠNG REPOSITORY (AUDIT REPORT)
**Dự án:** Buổi 12 – Modular Monolithic → Microservices (.NET 10)  
**Ngày thực hiện:** 2026-11-01 | **Người thực hiện:** Senior Backend Engineer & Reviewer

---

## 1. TỔNG QUAN ĐÁNH GIÁ
Sau khi rà soát toàn bộ mã nguồn và tài liệu hiện có trong repository, hệ thống đã phân rã được ranh giới 3 module (`Order`, `Inventory`, `Notification`) và có tư duy thiết kế tốt. Tuy nhiên, phiên bản trước mắc **5 nhóm lỗi nghiêm trọng** về tính chân thực kỹ thuật và mức độ đáp ứng đề bài thực chiến.

---

## 2. DANH SÁCH LỖI ĐÃ XÁC NHẬN

| # | Lỗi / Khiếm khuyết | Tệp tin liên quan | Mức độ | Tác động thực tế |
|---|---|---|:---:|---|
| **1** | **Giả lập Message Queue bằng RAM** thay vì kết nối RabbitMQ thật | `src/Shared/RabbitMQ/RabbitMqBroker.cs` | **CRITICAL** | Sử dụng `ConcurrentQueue<string>` trong bộ nhớ RAM, tự log "delivery_mode=2" và "durable". Không có Broker thật, không có AMQP socket. |
| **2** | **Lưu trữ JSON file thay vì CSDL PostgreSQL** đa schema | `src/Modules/*/Infrastructure/*DbContext.cs` | **CRITICAL** | Dùng `System.Text.Json` đọc/ghi file `.json`, không phản ánh đúng CSDL quan hệ, không có SQL transaction, không có phân tách schema `order`, `inventory`, `notification`. |
| **3** | **Chưa có cơ chế Chống bán vượt tồn (Anti-overselling)** | `src/Modules/Inventory/Domain/StockItem.cs`, `InventoryService.cs` | **HIGH** | Chỉ kiểm tra điều kiện dạng `if (stock >= qty)` rồi trả về bool. Khi 2 request tới cùng lúc sẽ xảy ra Race Condition bán âm kho. Chưa có quy trình: Kiểm tra – Giữ hàng (`Reserve`) – Trừ hàng (`Sold`) / Nhả hàng (`Release`). |
| **4** | **Thiếu chế độ Sync qua HTTP REST** giữa Order và Inventory | `src/Modules/Order/Application/OrderService.cs` | **HIGH** | Chỉ có 1 adapter in-process gọi RAM. Chưa có cấu hình `Inventory:Mode` (`InProcess` vs `Http`) để chứng minh việc đổi cách gọi mạng mà `OrderService` không đổi 1 dòng code. |
| **5** | **Tuyên bố quá mức (Overclaiming) & Số đo không kiểm chứng** | `README.md`, `slide_buoi_12.md`, `tai_lieu_nghien_cuu_buoi_12.md` | **MEDIUM** | Tuyên bố "Zero Message Loss 100%", "PASS 100%", gán số đo "< 5ms", "< 50ms" tùy tiện; slide ghi có gRPC nhưng code thực tế không có; chứa đường dẫn cục bộ `D:\...`. |
| **6** | **Thiếu bộ kiểm thử tự động xUnit & Script kiểm chứng tải** | Root / `demo` | **HIGH** | Chưa có project test xUnit (5 test bắt buộc); chưa có `race.ps1` kiểm tra tranh chấp đơn; chưa có `docker-compose.yml` cho Postgres & RabbitMQ. |

---

## 3. KẾ HOẠCH HÀNH ĐỘNG KHẮC PHỤC (P0)
1. **Hạ tầng Container thật:** Xây dựng `docker-compose.yml` với PostgreSQL 16 (tạo sẵn 3 schema `order`, `inventory`, `notification`) và RabbitMQ 3 Management (cổng 5672 và UI 15672).
2. **PostgreSQL & Dữ liệu thật:** Tái cấu trúc cả 3 module dùng Npgsql / EF Core với DbContext `internal`, kết nối tới đúng schema tương ứng. Seed sẵn 3 sản phẩm: Laptop (5), Chuột (0), Laptop-cuối (1).
3. **Quy trình Chống bán vượt tồn:** Triển khai câu lệnh SQL Atomic `UPDATE ... SET available = available - @qty, reserved = reserved + @qty WHERE product_id = @id AND available >= @qty`.
4. **Hai Adapter Sync:** `InProcessInventoryService` và `HttpInventoryService` qua `Inventory:Mode`.
5. **RabbitMQ thật:** Triển khai `RabbitMQ.Client`, durable fanout exchange `order.events`, durable queues, persistent message, manual ACK, dead-letter exchange/queue và xử lý idempotent theo `EventId`.
6. **Endpoints & Script Demo:** Bổ sung `/demo/consumers/notification/stop|start`, `/demo/reset`, `demo/race.ps1`, và bộ 5 unit/integration test xUnit.
7. **Hiệu chỉnh tài liệu:** Sửa toàn bộ slide, giáo trình và Q&A về đúng thực tế kỹ thuật, không phóng đại.
