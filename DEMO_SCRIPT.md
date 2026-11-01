# KỊCH BẢN THUYẾT TRÌNH DEMO BUỔI 12 (10 PHÚT)
**Học phần:** Phát triển Hệ thống Server Nâng cao  
**Chủ đề:** Modular Monolithic — Giao tiếp Đồng bộ & Bất đồng bộ với RabbitMQ  
**Câu chuyện xuyên suốt:** *"Khách hàng đặt mua chiếc Laptop Gaming cuối cùng"*

---

## 📋 THÔNG TIN CHUẨN BỊ TRƯỚC KHI BẮT ĐẦU
1. Khởi động hạ tầng: `docker compose up -d` (PostgreSQL 16 & RabbitMQ 3.13)
2. Khởi động Web API: `dotnet run --project demo/src/Host/ModularMonolith.Web.csproj --urls "http://localhost:5000"`
3. Mở sẵn 3 tab trình duyệt:
   - **Tab 1 (Root API):** `http://localhost:5000/` (Giới thiệu 3 module, schema CSDL)
   - **Tab 2 (Trạng thái hệ thống):** `http://localhost:5000/demo/status`
   - **Tab 3 (RabbitMQ Management):** `http://localhost:15672` (User/Pass: `guest` / `guest`)
4. Mở file `demo/requests.http` trong VS Code (hoặc Terminal chạy PowerShell).

---

## 🎬 9 BƯỚC THUYẾT TRÌNH DEMO CHI TIẾT

### BƯỚC 1: CẤU TRÚC MODULAR MONOLITH & CÔ LẬP SCHEMA (DEMO 1)
* **Thao tác:** 
  - Mở cây thư mục `demo/src/Modules/`: chỉ ra 3 thư mục `Order`, `Inventory`, `Notification`.
  - Mở file `init-db.sql`: chỉ ra 3 schema PostgreSQL độc lập `order`, `inventory`, `notification`.
  - Mở `Inventory.Contracts/IInventoryService.cs`: chỉ ra interface công khai duy nhất.
* **Kết quả mong đợi:** 
  - Khán giả thấy rõ 3 module độc lập nằm trong cùng 1 process, DbContext `internal`, cấm truy cập chéo CSDL.
* **Chỗ cần nhìn:** Cây thư mục VS Code và 3 schema trong CSDL.
* **Câu giải thích 1 dòng:**  
  > *"Modular Monolith giống như một tòa nhà chia 3 phòng khép kín: Phòng Đơn hàng, Phòng Kho và Phòng Thông báo; mỗi phòng giữ chìa khóa tủ hồ sơ riêng, tuyệt đối không ai được tự ý lục lọi tủ của phòng khác."*

---

### BƯỚC 2: GIAO TIẾP ĐỒNG BỘ — TÌNH HUỐNG ĐỦ HÀNG (DEMO 2.1)
* **Thao tác:** Gửi request đặt mua 2 chiếc Laptop Gaming Pro 16:
  ```http
  POST http://localhost:5000/api/orders
  {"customerId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "items": [{"productId": "11111111-1111-1111-1111-111111111111", "quantity": 2}]}
  ```
* **Kết quả mong đợi:** HTTP `201 Created`, trạng thái đơn `Confirmed`, tổng tiền 50.000.000 VNĐ.
* **Chỗ cần nhìn:** Log terminal `[ORDER] -> [INVENTORY] [RESERVE-SUCCESS] Giữ thành công 2 sản phẩm`.
* **Câu giải thích 1 dòng:**  
  > *"Giao tiếp đồng bộ là việc Order gọi điện thoại sang Kho hỏi 'còn hàng không?' và đứng máy chờ; Kho kiểm tra thấy đủ hàng và tạm khóa 2 chiếc trên sổ sách rồi báo lại ngay."*

---

### BƯỚC 3: GIAO TIẾP ĐỒNG BỘ — TÌNH HUỐNG HẾT HÀNG (DEMO 2.2)
* **Thao tác:** Gửi request đặt mua 1 chiếc Chuột Không Dây RGB (kho có 0 chiếc):
  ```http
  POST http://localhost:5000/api/orders
  {"customerId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb", "items": [{"productId": "22222222-2222-2222-2222-222222222222", "quantity": 1}]}
  ```
* **Kết quả mong đợi:** HTTP `409 Conflict`, trạng thái đơn `Rejected`, thông báo *"Tồn kho khả dụng không đủ"*.
* **Chỗ cần nhìn:** Mã HTTP `409 Conflict` trên màn hình và đơn hàng Rejected được lưu vết lịch sử.
* **Câu giải thích 1 dòng:**  
  > *"Khi Kho báo hết hàng, Order lập tức từ chối giao dịch, bảo vệ hệ thống không bao giờ nhận tiền của khách khi không có hàng để giao."*

---

### BƯỚC 4: ĐỔI SANG CHẾ ĐỘ SYNC HTTP REST (`Inventory:Mode = Http`)
* **Thao tác:**
  - Mở `appsettings.json`, đổi `"Inventory": { "Mode": "Http" }`.
  - Gửi lại request đặt mua Laptop.
* **Kết quả mong đợi:** Đơn hàng tạo thành công HTTP `201 Created` y hệt như chế độ InProcess.
* **Chỗ cần nhìn:** Log terminal `[INVENTORY-HTTP] [SYNC-REST] Gửi HTTP POST /internal/inventory/reserve` (gọi qua REST endpoint loopback).
* **Câu giải thích 1 dòng:**  
  > *"Nhờ ranh giới interface `IInventoryService`, mã nguồn của `OrderService` không phải thay đổi dù chỉ một ký tự khi chúng ta chuyển đổi từ gọi trong bộ nhớ sang gọi qua mạng REST API!"*

---

### BƯỚC 5: TẠO ĐƠN & QUAN SÁT MESSAGE TRÊN RABBITMQ MANAGEMENT UI
* **Thao tác:** Mở giao diện `http://localhost:15672` (Tab Exchanges $\rightarrow$ `order.events`).
* **Kết quả mong đợi:** 
  - Thấy Fanout Exchange `order.events` bền vững (Durable).
  - Có 2 hàng đợi gắn vào (Binding): `inventory.order-created` và `notification.order-created`.
* **Chỗ cần nhìn:** Cột Bindings và đồ thị Message in/out trên RabbitMQ UI.
* **Câu giải thích 1 dòng:**  
  > *"Sau khi chốt đơn, Order thả sự kiện OrderCreated vào hòm thư Fanout Exchange; Exchange sẽ tự động nhân bản thông điệp thành 2 bản và gửi tới từng phòng ban quan tâm."*

---

### BƯỚC 6: DỪNG CONSUMER NOTIFICATION — THỰC NGHIỆM CONSUMER OFFLINE (DEMO 3.1)
* **Thao tác:** Gửi request dừng Consumer Notification:
  ```http
  POST http://localhost:5000/demo/consumers/notification/stop
  ```
  Sau đó gửi 1 request đặt mua thêm 1 Laptop:
  ```http
  POST http://localhost:5000/api/orders
  {"customerId": "cccccccc-cccc-cccc-cccc-cccccccccccc", "items": [{"productId": "11111111-1111-1111-1111-111111111111", "quantity": 1}]}
  ```
* **Kết quả mong đợi:** Khách hàng vẫn nhận HTTP `201 Created` ngay lập tức!
* **Chỗ cần nhìn:** Gọi `GET /demo/status`: thấy `notificationConsumer.status = "OFFLINE"`, và `notificationQueue_Depth = 1` (Ready = 1).
* **Câu giải thích 1 dòng:**  
  > *"Dù phòng Thông báo đang tạm đóng cửa nghỉ trưa, đơn hàng của khách vẫn hoàn tất mượt mà vì thông điệp đã được gửi vào hòm thư RabbitMQ nằm chờ an toàn trên đĩa cứng."*

---

### BƯỚC 7: BẬT LẠI CONSUMER NOTIFICATION — XỬ LÝ HỒI PHỤC (DEMO 3.2)
* **Thao tác:** Gửi request bật lại Consumer Notification:
  ```http
  POST http://localhost:5000/demo/consumers/notification/start
  ```
* **Kết quả mong đợi:** 
  - `GET /demo/status`: `notificationQueue_Depth` lập tức giảm từ 1 về **0**!
  - `GET /api/notifications`: Xuất hiện bản ghi email xác nhận cho đơn hàng vừa đặt trong lúc offline.
* **Chỗ cần nhìn:** Log terminal `[NOTIFICATION] [EMAIL-SIMULATED] (Mô phỏng) Đã gửi Email xác nhận...` và gửi `basicAck`.
* **Câu giải thích 1 dòng:**  
  > *"Khi phòng Thông báo mở cửa trở lại, nhân viên lập tức lấy thư tồn trong hòm ra xử lý và gửi email xác nhận cho khách, dữ liệu không hề bị rơi rớt."*

---

### BƯỚC 8: CHỐNG BÁN VƯỢT TỒN — CHẠY SCRIPT TRANH CHẤP `race.ps1`
* **Thao tác:** Chạy script PowerShell kiểm tra tranh chấp đồng thời:
  ```powershell
  powershell -File demo/race.ps1
  ```
* **Kết quả mong đợi:** 
  - Khách 1 nhận HTTP `201 Created` (mua được chiếc laptop cuối cùng).
  - Khách 2 nhận HTTP `409 Conflict` (báo hết hàng).
  - Tồn kho khả dụng sau giao dịch: đúng bằng `0`.
* **Chỗ cần nhìn:** Kết quả in ra terminal: `✔ XÁC NHẬN THÀNH CÔNG: Chống bán vượt tồn hoạt động 100% chuẩn xác! (Đúng 1x201 và 1x409)`.
* **Câu giải thích 1 dòng:**  
  > *"Nhờ câu lệnh UPDATE nguyên tử ở bước Giữ hàng, hệ thống khóa chặt điều kiện tồn kho; kể cả khi 2 người cùng bấm nút mua vào cùng một phần nghìn giây, chỉ duy nhất người đến trước mua được hàng!"*

---

### BƯỚC 9: GIẢI THÍCH LỘ TRÌNH TÁCH INVENTORY THÀNH MICROSERVICE (PHẦN 4)
* **Thao tác:** Chiếu slide Lộ trình 4 bước và chỉ vào mã nguồn:
  1. *Database riêng:* Schema `inventory` đã độc lập, chỉ cần mang file script sang cụm PostgreSQL mới.
  2. *Giao tiếp mạng:* Chỉ cần cấu hình `Inventory:Mode = Http` trỏ tới URL của microservice mới.
  3. *Docker riêng:* Viết Dockerfile đóng gói riêng project `Modules.Inventory`.
  4. *CI/CD riêng:* Thiết lập workflow GitHub Actions với trigger path `src/Modules/Inventory/**`.
* **Kết quả mong đợi:** Hội đồng công nhận tính khả thi thực tế và nguyên lý giữ nguyên 100% logic nghiệp vụ.
* **Câu giải thích 1 dòng:**  
  > *"Microservice không phải là đích đến bắt buộc; nhưng nếu ngày mai đợt sale 11.11 khiến kho bị nghẽn, chúng ta có thể bóc tách phòng Kho sang một tòa nhà riêng trong vòng nửa ngày mà không cần viết lại mã nghiệp vụ!"*
