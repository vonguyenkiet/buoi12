# DỰ ÁN DEMO SẢN PHẨM BUỔI 12
# MODULAR MONOLITHIC & GIAO TIẾP ĐỒNG BỘ / BẤT ĐỒNG BỘ VỚI RABBITMQ

**Học phần:** Phát triển Hệ thống Server Nâng cao — Buổi 12  
**Nền tảng:** .NET 10 (C#) — Kiến trúc Modular Monolithic  
**Thư mục dự án:** `D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo`

---

## 1. TỔNG QUAN KIẾN TRÚC PROJECT DEMO

Solution `ModularMonolithDemo.slnx` được tổ chức bám sát 100% tài liệu môn học và 4 yêu cầu demo bắt buộc:

```
demo/
├── ModularMonolithDemo.slnx                 # Solution quản lý toàn bộ các project
├── src/
│   ├── Shared/
│   │   └── Shared.Kernel/                  # Event Contracts & Bộ giả lập AMQP RabbitMQ Broker
│   │       ├── Events/OrderCreatedEvent.cs
│   │       └── RabbitMQ/RabbitMqBroker.cs  # Hỗ trợ Fanout, Direct, Topic, Queue Durability, Manual ACK
│   ├── Modules/
│   │   ├── Inventory/                      # Module Tồn kho & SKU vật lý
│   │   │   ├── Contracts/IInventoryService.cs # Public Interface (Architectural Seam)
│   │   │   ├── Domain/StockItem.cs
│   │   │   ├── Infrastructure/InventoryDbContext.cs (Internal - Isolated Schema)
│   │   │   ├── Application/InventoryService.cs
│   │   │   └── Consumers/InventoryOrderCreatedConsumer.cs
│   │   ├── Order/                          # Module Đơn hàng (Core Domain)
│   │   │   ├── Domain/Order.cs & OrderLineItem.cs (Snapshot Pattern)
│   │   │   ├── Infrastructure/OrderDbContext.cs (Internal - Isolated Schema)
│   │   │   └── Application/OrderService.cs (Điều phối Sync Call & Async Publish)
│   │   └── Notification/                   # Module Gửi Thông báo (Email / SMS)
│   │       ├── Domain/NotificationLog.cs
│   │       ├── Infrastructure/NotificationDbContext.cs (Internal - Isolated Schema)
│   │       └── Consumers/NotificationOrderCreatedConsumer.cs (Manual ACK)
│   ├── Host/
│   │   └── ModularMonolith.Web/            # ASP.NET Core Web API (HTTP REST Endpoints)
│   └── Runner/
│       └── ModularMonolith.Runner/         # Console CLI thực thi tự động toàn bộ 4 phần Demo
```

---

## 2. HƯỚNG DẪN CHẠY DEMO

### Cách 1: Chạy Tự Động Toàn Bộ 4 Phần Demo (Khuyến nghị khi Thuyết trình)
Mở terminal tại thư mục `demo/` và thực thi:
```bash
cd D:/Hocki7/PtrienHeThongServerNC/LyThuyet/Buoi12/demo
dotnet run --project src/Runner/ModularMonolith.Runner.csproj
```

**Màn hình sẽ tự động thực thi và trực quan hóa 4 phần demo:**
1. **Phần 1 - Cấu trúc Modular Monolithic:**
   - Trình bày 3 module tách biệt (`Order`, `Inventory`, `Notification`).
   - Chứng minh cô lập CSDL vật lý qua 3 schema file riêng biệt:
     + `inventory_schema.json`
     + `order_schema.json`
     + `notification_schema.json`
   - Chứng minh cấm truy cập chéo DbContext (được khai báo `internal`).
   - Chứng minh không có SQL JOIN chéo bảng (áp dụng Snapshot Pattern).
2. **Phần 2 - Lời gọi Đồng bộ (Sync) kiểm tra tồn kho:**
   - **Tình huống 2.1 (Đủ hàng):** Đặt mua 2 Laptop (kho còn 5) $\rightarrow$ Gọi đồng bộ `IInventoryService.CheckStockAsync()` $\rightarrow$ Trả về `IsAvailable = true` $\rightarrow$ Tạo đơn thành công (Status: `Confirmed`).
   - **Tình huống 2.2 (Hết hàng):** Đặt mua 1 Chuột RGB (kho có 0) $\rightarrow$ Gọi đồng bộ `IInventoryService.CheckStockAsync()` $\rightarrow$ Trả về `IsAvailable = false` $\rightarrow$ Từ chối tạo đơn ngay lập tức (Status: `Rejected`, Out of stock).
3. **Phần 3 - Lời gọi Bất đồng bộ (Async) với RabbitMQ & Kịch bản Consumer Offline:**
   - Phát sự kiện `OrderCreatedEvent` (`delivery_mode = 2` Persistent) lên Fanout Exchange `ecommerce.order-events`.
   - Cả 2 Consumer (`Inventory` và `Notification`) cùng nhận và xử lý song song ngầm.
   - **Thực nghiệm tắt Consumer:**
     + Tắt `Notification Service` (Consumer Offline).
     + Khách đặt thêm 1 đơn hàng Laptop mới $\rightarrow$ Order Service tạo đơn và bắn sự kiện thành công!
     + Kiểm tra Broker: Queue `order.notification` lưu trữ message trên đĩa (`Message Ready = 1`), dữ liệu an toàn 100%!
     + Khởi động lại `Notification Service` $\rightarrow$ Tự động kéo message về, gửi Email & SMS, gửi `basicAck` $\rightarrow$ Hàng đợi về `0`!
4. **Phần 4 - Giải thích trước lớp về Con đường tiến hóa lên Microservices:**
   - Chỉ rõ nhờ nguyên lý Dependency Inversion và interface `IInventoryService`, toàn bộ mã nguồn Domain & Application được giữ nguyên 100%.
   - Trình bày 4 bước bóc tách module Inventory thành Microservice độc lập.

---

### Cách 2: Khởi Chạy ASP.NET Core Web API Server
Mở terminal và thực thi:
```bash
cd D:/Hocki7/PtrienHeThongServerNC/LyThuyet/Buoi12/demo
dotnet run --project src/Host/ModularMonolith.Web.csproj
```

Server sẽ lắng nghe tại `http://localhost:5000` (hoặc cổng ngẫu nhiên hiển thị trên terminal).

**Các API có thể test bằng Postman hoặc curl:**
1. **Kiểm tra thông tin sản phẩm và kho:**
   ```bash
   curl http://localhost:5000/api/inventory/11111111-1111-1111-1111-111111111111
   ```
2. **Đặt đơn hàng thành công (Còn hàng):**
   ```bash
   curl -X POST http://localhost:5000/api/orders \
     -H "Content-Type: application/json" \
     -d '{"customerId":"3fa85f64-5717-4562-b3fc-2c963f66afa6","items":[{"productId":"11111111-1111-1111-1111-111111111111","productName":"Laptop Pro","quantity":1,"unitPrice":25000000}]}'
   ```
3. **Đặt đơn hàng thất bại (Hết hàng - Chuột RGB ID 22222222-...):**
   ```bash
   curl -X POST http://localhost:5000/api/orders \
     -H "Content-Type: application/json" \
     -d '{"customerId":"3fa85f64-5717-4562-b3fc-2c963f66afa6","items":[{"productId":"22222222-2222-2222-2222-222222222222","productName":"Chuot RGB","quantity":1,"unitPrice":450000}]}'
   ```
4. **Xem lịch sử gửi thông báo Email/SMS từ RabbitMQ Event:**
   ```bash
   curl http://localhost:5000/api/notifications
   ```
5. **Bật / Tắt Consumer Notification để test kịch bản Offline:**
   ```bash
   curl -X POST http://localhost:5000/api/demo/offline-consumer-toggle
   ```

---

## 3. ĐỐI CHIẾU TIÊU CHUẨN ĐÁNH GIÁ (RUBRIC MÔN HỌC)

| Tiêu chí Rubric môn học | Cách hiện thực trong Demo | Kết quả kiểm thử |
|---|---|:---:|
| **1. Cấu trúc Modular Monolith** | 3 module riêng biệt trong cùng 1 process, 3 schema lưu trữ độc lập, DbContext khai báo internal | **ĐẠT (PASS)** |
| **2. Demo Lời gọi Đồng bộ (Sync)** | Order gọi `IInventoryService.CheckStockAsync`, test cả 2 case: Đủ hàng (Confirmed) và Hết hàng (Rejected) | **ĐẠT (PASS)** |
| **3. Demo Bất đồng bộ với RabbitMQ** | Publish `OrderCreatedEvent` lên Fanout Exchange, 2 Consumer xử lý song song, demo tắt Consumer không mất tin | **ĐẠT (PASS)** |
| **4. Giải thích Tiến hóa Microservices** | Giữ nguyên 100% logic nghiệp vụ của Domain/Application, chỉ thay DI gRPC Client cho `IInventoryService` | **ĐẠT (PASS)** |

Dự án demo đã sẵn sàng để trình diễn và bảo vệ trực tiếp trước Giảng viên và Hội đồng!
