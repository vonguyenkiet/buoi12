# BUỔI 12: MODULAR MONOLITHIC – TƯ DUY THIẾT KẾ VÀ CON ĐƯỜNG TIẾN HÓA LÊN MICROSERVICES

**Học phần:** Phát triển Hệ thống Server Nâng cao  
**Đơn vị:** Bộ môn Kỹ thuật Phần mềm — Khoa Công nghệ Thông tin  
**Nhóm thực hiện:** Nhóm nghiên cứu Lý thuyết & Thực hành Buổi 12  

---

## 📌 TỔNG QUAN DỰ ÁN

Dự án này là bộ tài liệu hoàn chỉnh và mã nguồn thực thi mẫu cho chuyên đề **Buổi 12: Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservice**, được tổ chức khoa học theo thế **"Kiềng 3 chân"**:
1. **Kiến trúc vĩ mô & Lộ trình tiến hóa (TV1):** So sánh Monolith truyền thống, Microservices và Modular Monolithic; Định luật Conway; Nguyên tắc MonolithFirst; Lộ trình chuẩn 4 bước bóc tách module mà không cần viết lại logic nghiệp vụ.
2. **Ranh giới nghiệp vụ & Kỷ luật cô lập dữ liệu (TV2):** Domain-Driven Design (DDD), Bounded Context, Ubiquitous Language; Ma trận 10 nghiệp vụ E-commerce áp dụng Bộ 4 câu hỏi vàng; 4 kỷ luật cô lập CSDL thép (cấm SQL JOIN chéo, cấm dùng chung DbContext, cấm khóa ngoại cứng); Public Service Interface.
3. **Cơ chế giao tiếp liên module (TV3):** Giao tiếp đồng bộ (gRPC HTTP/2 Protobuf vs RESTful API) dẹp bỏ Cascading Failure; Giao tiếp bất đồng bộ qua RabbitMQ AMQP 0-9-1 (Fanout Pub/Sub, Queue Durability, Message Persistence, Manual ACK); Luồng thực chiến `OrderCreated` kết hợp nhịp nhàng Sync & Async.

---

## 📁 CẤU TRÚC THƯ MỤC REPOSITORY

```
.
├── docker-compose.yml                 # Khởi chạy PostgreSQL 16 & RabbitMQ 3.13 Management
├── init-db.sql                        # Script DDL khởi tạo sẵn 3 schema độc lập và các bảng
├── .env.example                       # Cấu hình môi trường mẫu (không chứa mật khẩu thật)
├── DEMO_SCRIPT.md                     # Kịch bản 9 bước thuyết trình demo chuẩn 10 phút
├── slide_buoi_12.pptx                 # [Slide Thuyết trình] 20 slide PowerPoint tông trắng tối giản, đủ Speaker Notes
├── slide_buoi_12.md                   # [Slide Markdown] Định dạng Marp sẵn sàng trình chiếu
├── tai_lieu_nghien_cuu_buoi_12.md     # [Giáo trình Lý thuyết] Báo cáo hàn lâm chuyên sâu 5 chương
├── ngan_hang_cau_hoi_phan_bien_buoi_12.md # [Ngân hàng Q&A] 15 câu hỏi phản biện hội đồng chia đều 4 mảng
├── docs/
│   ├── AUDIT.md                       # Báo cáo kiểm toán hiện trạng kỹ thuật
│   └── EVIDENCE.md                    # Bằng chứng thực thi thật (Logs, Output, Test run)
├── demo/
│   ├── ModularMonolithDemo.slnx       # Solution .NET 10 quản lý các projects
│   ├── requests.http                  # Bộ kịch bản HTTP requests test từng bước demo
│   ├── race.ps1                       # Script PowerShell kiểm chứng tranh chấp đồng thời
│   ├── src/
│   │   ├── Host/ModularMonolith.Web/  # ASP.NET Core Web API (Endpoints chính & Demo endpoints)
│   │   ├── Modules/
│   │   │   ├── Inventory.Contracts/   # Public Interface IInventoryService & DTOs
│   │   │   ├── Inventory/             # Module Quản lý Tồn kho & SKU (Schema 'inventory')
│   │   │   ├── Order/                 # Module Xử lý Đơn hàng (Schema 'order')
│   │   │   └── Notification/          # Module Gửi Thông báo (Schema 'notification')
│   │   └── Shared/Shared.Kernel/      # RabbitMQ Connection & Event Contracts
│   └── tests/
│       └── ModularMonolith.Tests/     # Bộ 5 bài test tự động xUnit & NetArchTest
├── Buổi 12 Modular Monolithic...pdf   # Tài liệu gốc của môn học
├── Kế Hoạch Phân Công...pdf           # Kế hoạch phân công nghiên cứu lý thuyết
└── Phan_cong_ly_thuyet_Buoi_12.pdf    # Quy định phân công nhóm
```

---

## 🚀 HƯỚNG DẪN CHẠY DEMO & KIỂM CHỨNG

### Bước 1: Khởi động Hạ tầng Docker (PostgreSQL & RabbitMQ)
```bash
docker compose up -d
```
* PostgreSQL lắng nghe cổng `5432` (tự động tạo 3 schema: `inventory`, `order`, `notification`).
* RabbitMQ lắng nghe cổng `5672` (AMQP) và `15672` (Management Dashboard tại `http://localhost:15672`).

### Bước 2: Chạy Server Web API
```bash
dotnet run --project demo/src/Host/ModularMonolith.Web.csproj --urls "http://localhost:5000"
```
* Xem thông tin hệ thống tại `http://localhost:5000/`.
* Xem trạng thái hàng đợi và tồn kho tại `http://localhost:5000/demo/status`.

### Bước 3: Chạy Bộ Test Tự Động xUnit
Mở terminal thứ hai và thực thi:
```bash
dotnet test demo/ModularMonolithDemo.slnx
```
Kết quả kiểm chứng thực tế: **Passed 5/5 tests** (Đủ hàng 201, Hết hàng 409, Tranh chấp đồng thời, Ranh giới kiến trúc NetArchTest, và Kỷ luật CSDL internal).

---

## 📝 GIẤY PHÉP & BẢN QUYỀN
Tài liệu và mã nguồn phục vụ mục đích học tập và nghiên cứu trong khuôn khổ Học phần Phát triển Hệ thống Server Nâng cao.
