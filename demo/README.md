# DỰ ÁN DEMO BUỔI 12: MODULAR MONOLITHIC & GIAO TIẾP ĐỒNG BỘ / BẤT ĐỒNG BỘ

**Học phần:** Phát triển Hệ thống Server Nâng cao — Buổi 12  
**Nền tảng:** .NET 10 (C#), PostgreSQL 16, RabbitMQ 3.13  

---

## 1. TỔNG QUAN KIẾN TRÚC MÃ NGUỒN

Solution `ModularMonolithDemo.slnx` tổ chức 3 module độc lập theo Clean Architecture thu nhỏ và Domain-Driven Design:

```
demo/
├── ModularMonolithDemo.slnx                 # Solution .NET 10 quản lý các projects
├── requests.http                            # File kịch bản gọi API từng bước cho VS Code
├── race.ps1                                 # Script PowerShell kiểm tra tranh chấp đồng thời
├── src/
│   ├── Host/ModularMonolith.Web/            # ASP.NET Core Web API Server
│   ├── Modules/
│   │   ├── Inventory.Contracts/             # Public Service Interface IInventoryService & DTOs
│   │   ├── Inventory/                       # Quản lý Kho, SKU & Atomic Reserve (Schema: 'inventory')
│   │   ├── Order/                           # Xử lý Đơn hàng & Snapshot Pattern (Schema: 'order')
│   │   └── Notification/                    # Gửi Thông báo mô phỏng (Schema: 'notification')
│   └── Shared/Shared.Kernel/                # Event Contracts & RabbitMQ AMQP 0-9-1 Connection
└── tests/
    └── ModularMonolith.Tests/               # Bộ 5 bài test tự động xUnit & NetArchTest
```

---

## 2. HƯỚNG DẪN KHỞI CHẠY & KIỂM THỬ

### Bước 1: Khởi động Hạ tầng Docker (PostgreSQL & RabbitMQ)
Từ thư mục gốc dự án:
```bash
docker compose up -d
```

### Bước 2: Khởi chạy Máy chủ Web API
```bash
dotnet run --project src/Host/ModularMonolith.Web.csproj --urls "http://localhost:5000"
```

### Bước 3: Chạy Bộ Test Tự Động xUnit
```bash
dotnet test ModularMonolithDemo.slnx
```

---

## 3. THỰC HIỆN 9 BƯỚC DEMO
Vui lòng tham khảo tệp `DEMO_SCRIPT.md` tại thư mục gốc để thực hiện đúng 9 bước thuyết trình chuẩn 10 phút.
Các request mẫu đã được chuẩn bị đầy đủ trong tệp `requests.http`.
Script kiểm thử tranh chấp đồng thời: `powershell -File race.ps1`.