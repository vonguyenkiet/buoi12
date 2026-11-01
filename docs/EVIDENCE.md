# BẰNG CHỨNG KIỂM CHỨNG KỸ THUẬT THỰC TẾ (TECHNICAL EVIDENCE)
**Dự án:** Buổi 12 – Modular Monolithic → Microservices (.NET 10)  
**Thời gian kiểm chứng:** 2026-11-01  
**Môi trường:** Windows 11, .NET 10 SDK (10.0.401), Docker Desktop (PostgreSQL 16, RabbitMQ 3.13)

---

## 1. BẢNG TRẠNG THÁI KIỂM CHỨNG HẠ TẦNG & MÃ NGUỒN

| Hạng mục kiểm chứng | Phương pháp kiểm thử | Kết quả thực tế | Trạng thái |
|---|---|---|:---:|
| **PostgreSQL 16 (3 Schemas)** | Query `information_schema.schemata` | Tồn tại đủ 3 schema: `inventory`, `order`, `notification` | **PASS** |
| **RabbitMQ 3.13 AMQP & UI** | Ping cổng 5672 và Management UI 15672 | Đã kết nối, exchange `order.events` và 2 durable queues | **PASS** |
| **5/5 Tests Tự động xUnit** | `dotnet test ModularMonolithDemo.slnx` | Passed 5/5 tests trong 4 giây | **PASS** |
| **Ranh giới kiến trúc (NetArchTest)**| `Test4_KienTruc_OrderChiPhuThuocInterface`| Order không phụ thuộc mã nội bộ Inventory | **PASS** |
| **Kỷ luật cô lập CSDL** | `Test5_KienTruc_DbContextInternal` | 100% DbContext là `internal` | **PASS** |
| **Chống bán vượt tồn (Concurrency)**| `Test3` & script `demo/race.ps1` | Đúng 1x201 Created và 1x409 Conflict | **PASS** |
| **Kịch bản Consumer Offline** | Dừng Consumer qua API $\rightarrow$ Tạo đơn | Queue depth `notification.order-created` = 1 (Ready=1) | **PASS** |
| **Hồi phục Consumer Online** | Bật lại Consumer qua API | Queue depth trở về 0, email mô phỏng ghi vào DB | **PASS** |
| **Bảo mật Khóa / Mật khẩu** | Rà soát `.gitignore` và codebase | Không có password thật, không có secret trong repo | **PASS** |

---

## 2. BẰNG CHỨNG THỰC THI BỘ TEST XUNIT (LOG THẬT)

```text
D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo> dotnet test ModularMonolithDemo.slnx
Determining projects to restore...
  All projects are up-to-date for restore.
  Inventory.Contracts -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Modules\Inventory.Contracts\bin\Debug\net10.0\Inventory.Contracts.dll
  Shared.Kernel -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Shared\bin\Debug\net10.0\Shared.Kernel.dll
  Modules.Inventory -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Modules\Inventory\bin\Debug\net10.0\Modules.Inventory.dll
  Modules.Order -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Modules\Order\bin\Debug\net10.0\Modules.Order.dll
  Modules.Notification -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Modules\Notification\bin\Debug\net10.0\Modules.Notification.dll
  ModularMonolith.Web -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\src\Host\bin\Debug\net10.0\ModularMonolith.Web.dll
  ModularMonolith.Tests -> D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\tests\ModularMonolith.Tests\bin\Debug\net10.0\ModularMonolith.Tests.dll
Test run for D:\Hocki7\PtrienHeThongServerNC\LyThuyet\Buoi12\demo\tests\ModularMonolith.Tests\bin\Debug\net10.0\ModularMonolith.Tests.dll (.NETCoreApp,Version=v10.0)

Passed!  - Failed:     0, Passed:     5, Skipped:     0, Total:     5, Duration: 4 s - ModularMonolith.Tests.dll (net10.0)
```

---

## 3. BẰNG CHỨNG GIAO TIẾP ĐỒNG BỘ: ĐỦ HÀNG (201) vs HẾT HÀNG (409)

### Yêu cầu 1: Đủ hàng (2 chiếc Laptop Gaming Pro 16)
```http
POST /api/orders HTTP/1.1
Content-Type: application/json
{"customerId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "items": [{"productId": "11111111-1111-1111-1111-111111111111", "quantity": 2}]}

HTTP/1.1 201 Created
Location: /api/orders/aff3c1ac-d769-4073-8ce0-2e5a2e82257f
{"success":true,"orderId":"aff3c1ac-d769-4073-8ce0-2e5a2e82257f","status":0,"totalAmount":50000000.00,"message":"Đặt hàng thành công! Đơn hàng đã được xác nhận và chuyển tiếp xử lý ngầm."}
```

### Yêu cầu 2: Hết hàng (1 chiếc Chuột RGB tồn kho = 0)
```http
POST /api/orders HTTP/1.1
Content-Type: application/json
{"customerId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", "items": [{"productId": "22222222-2222-2222-2222-222222222222", "quantity": 1}]}

HTTP/1.1 409 Conflict
{"success":false,"orderId":"78b54329-1bc8-48a8-bb9c-7640a666ac3c","status":1,"totalAmount":0,"message":"Tồn kho khả dụng không đủ (Hiện có: 0, Yêu cầu: 1)."}
```

---

## 4. BẰNG CHỨNG CONSUMER OFFLINE: TIN NHẮN CHỜ TRONG HÀNG ĐỢI RABBITMQ

### Bước 1: Dừng Consumer Notification
```http
POST /demo/consumers/notification/stop HTTP/1.1
HTTP/1.1 200 OK
{"status":"STOPPED","isActive":false,"message":"Notification Consumer đã dừng! Các thông điệp OrderCreated mới sẽ nằm chờ trong hàng đợi RabbitMQ (Ready > 0)."}
```

### Bước 2: Tạo đơn hàng mới trong lúc Consumer OFFLINE
```http
POST /api/orders HTTP/1.1
HTTP/1.1 201 Created
{"success":true,"orderId":"86d8d5fe-7ff2-4412-8e02-896d41ddd3f4",...}
```

### Bước 3: Kiểm tra hàng đợi RabbitMQ (`GET /demo/status`)
```json
{
  "notificationConsumer": {
    "queue": "notification.order-created",
    "isOnline": false,
    "status": "OFFLINE"
  },
  "rabbitMqQueues": {
    "inventoryQueue_Depth": 0,
    "notificationQueue_Depth": 1,
    "deadLetterQueue_Depth": 0
  }
}
```
*Kết quả:* Thông điệp được RabbitMQ bảo toàn nguyên vẹn trên đĩa cứng (`notificationQueue_Depth = 1`).

### Bước 4: Khởi động lại Consumer Notification
```http
POST /demo/consumers/notification/start HTTP/1.1
HTTP/1.1 200 OK
{"status":"RUNNING","isActive":true,"message":"Notification Consumer đã bật lại! Đang tiêu thụ và xử lý toàn bộ thông điệp tồn đọng trong queue."}
```
*Kết quả sau hồi phục:* `notificationQueue_Depth` trở về `0`, bản ghi email xác nhận xuất hiện trong `notification.notification_logs`:
```json
[
  {
    "id": "60a53fcb-f68f-4645-a7bc-e1faadfaf45b",
    "orderId": "86d8d5fe-7ff2-4412-8e02-896d41ddd3f4",
    "channel": "EMAIL",
    "recipient": "customer_cccccc@example.com",
    "message": "[MÔ PHỎNG] Đơn hàng #86d8d5fe tổng tiền 25.000.000 VNĐ đã được tiếp nhận thành công!",
    "isSimulation": true
  }
]
```

---

## 5. GIỚI HẠN HIỆN TẠI CỦA DỰ ÁN
1. **Gửi Email / SMS:** Triển khai ở mức **Mô phỏng (Simulation)** ghi log và CSDL, chưa tích hợp SMTP Server hay Twilio SMS thật để tránh phụ thuộc kết nối bên ngoài.
2. **Transactional Outbox Pattern:** Sự kiện `OrderCreated` được publish trực tiếp lên RabbitMQ sau khi commit CSDL. Giải pháp Outbox Table với background worker publish là bước nâng cấp mở rộng (P1).
3. **gRPC Protocol:** Demo tập trung vào HTTP REST cho giao tiếp mạng; gRPC là phương án khuyến nghị tối ưu khi bóc tách hoàn toàn sang microservice độc lập.
