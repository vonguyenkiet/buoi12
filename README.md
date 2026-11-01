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
├── slide_buoi_12.pptx                 # [Slide Thuyết trình] 32 slide PowerPoint tông trắng tối giản, đầy đủ Speaker Notes
├── slide_buoi_12.md                   # [Slide Markdown] Định dạng chuẩn Marp sẵn sàng trình chiếu
├── tai_lieu_nghien_cuu_buoi_12.md     # [Tài liệu Nghiên cứu] Báo cáo hàn lâm chuyên sâu 5 chương hoàn chỉnh
├── ngan_hang_cau_hoi_phan_bien_buoi_12.md # [Ngân hàng Q&A] 15 câu hỏi phản biện hội đồng chia đều 4 mảng
├── demo/                              # [Mã Nguồn Demo] Giải pháp Modular Monolithic mẫu trên nền tảng .NET 10 (C#)
│   ├── ModularMonolithDemo.slnx       # Solution .NET 10 quản lý các projects
│   ├── README.md                      # Hướng dẫn chi tiết chạy 4 phần demo
│   └── src/
│       ├── Shared/Shared.Kernel/      # Event contracts & Mô phỏng RabbitMQ Broker
│       ├── Modules/
│       │   ├── Inventory/             # Module Quản lý Tồn kho & SKU (Schema riêng)
│       │   ├── Order/                 # Module Xử lý Đơn hàng (Snapshot Pattern, Schema riêng)
│       │   └── Notification/          # Module Gửi Thông báo Email/SMS (Schema riêng)
│       ├── Host/ModularMonolith.Web/  # ASP.NET Core Web API (REST Endpoints)
│       └── Runner/ModularMonolith.Runner/ # CLI Console tự động chạy kịch bản 4 demo bắt buộc
├── Buổi 12 Modular Monolithic...pdf   # Tài liệu gốc của môn học
├── Kế Hoạch Phân Công...pdf           # Kế hoạch phân công nghiên cứu lý thuyết
└── Phan_cong_ly_thuyet_Buoi_12.pdf    # Quy định phân công nhóm
```

---

## 🚀 HƯỚNG DẪN CHẠY DEMO

Yêu cầu môi trường: Cài đặt **.NET 10 SDK** (hoặc .NET 8/9 trở lên).

### 1. Chạy Trình Diễn Tự Động Toàn Bộ 4 Phần Demo (Dành cho Buổi Thuyết trình)
```bash
cd demo
dotnet run --project src/Runner/ModularMonolith.Runner.csproj
```
Màn hình console sẽ tự động thực thi và trực quan hóa:
- **Demo 1:** Bằng chứng cô lập 3 schema CSDL (`inventory_schema.json`, `order_schema.json`, `notification_schema.json`), không có SQL JOIN chéo, DbContext nội bộ.
- **Demo 2:** Lời gọi đồng bộ kiểm tra tồn kho (Trường hợp đủ hàng $\rightarrow$ tạo đơn thành công; Trường hợp hết hàng $\rightarrow$ từ chối tạo đơn ngay lập tức).
- **Demo 3:** Lời gọi bất đồng bộ qua RabbitMQ Fanout Pub/Sub và **thực nghiệm tắt Consumer Notification** để chứng minh dữ liệu lưu trữ bền vững trên đĩa cứng (**Zero Message Loss**).
- **Demo 4:** Trực quan hóa con đường tiến hóa bóc tách module Inventory thành Microservice mà không sửa đổi một dòng code nghiệp vụ nào.

### 2. Chạy Web API Server
```bash
cd demo
dotnet run --project src/Host/ModularMonolith.Web.csproj
```
Truy cập `http://localhost:5000` để tương tác với các API đặt hàng và quản lý kho.

---

## 📝 GIẤY PHÉP & BẢN QUYỀN
Tài liệu và mã nguồn phục vụ mục đích học tập và nghiên cứu trong khuôn khổ Học phần Phát triển Hệ thống Server Nâng cao.
