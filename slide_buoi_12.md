---
marp: true
theme: gaia
_class: lead
paginate: true
backgroundColor: #ffffff
header: "Phát triển Hệ thống Server Nâng cao — Buổi 12: Modular Monolithic"
footer: "Nhóm nghiên cứu Lý thuyết Buổi 12 | Bộ môn Kỹ thuật Phần mềm"
style: |
  section {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
    font-size: 20px;
    padding-top: 52px;
    padding-bottom: 26px;
    padding-left: 42px;
    padding-right: 42px;
    color: #1e293b;
    background-color: #ffffff;
    line-height: 1.4;
  }
  header {
    top: 16px;
    left: 42px;
    font-size: 12px;
    color: #94a3b8;
    font-weight: 500;
  }
  footer {
    bottom: 14px;
    left: 42px;
    right: 42px;
    font-size: 11px;
    color: #94a3b8;
  }
  h1 {
    color: #0f172a;
    font-size: 28px;
    font-weight: 700;
    margin-bottom: 12px;
    padding-bottom: 5px;
    border-bottom: 1.5px solid #e2e8f0;
    letter-spacing: -0.02em;
  }
  h2 {
    color: #1e40af;
    font-size: 22px;
    font-weight: 600;
    margin-bottom: 10px;
  }
  h3 {
    color: #334155;
    font-size: 19px;
    font-weight: 600;
    margin-bottom: 6px;
  }
  .highlight {
    background-color: #f8fafc;
    padding: 8px 12px;
    border: 1px solid #e2e8f0;
    border-left: 4px solid #2563eb;
    border-radius: 4px;
    margin: 8px 0;
    font-size: 18px;
    color: #1e293b;
  }
  .warning-box {
    background-color: #fffdf5;
    padding: 8px 12px;
    border: 1px solid #fef3c7;
    border-left: 4px solid #d97706;
    border-radius: 4px;
    margin: 8px 0;
    font-size: 18px;
    color: #92400e;
  }
  .grid-2 {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 16px;
  }
  .grid-3 {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    gap: 12px;
  }
  table {
    font-size: 14px;
    border-collapse: collapse;
    width: 100%;
    margin: 6px 0;
    background-color: #ffffff;
  }
  th {
    background-color: #f1f5f9;
    color: #0f172a;
    padding: 5px 8px;
    text-align: left;
    font-weight: 600;
    border-bottom: 2px solid #cbd5e1;
    border-top: 1px solid #e2e8f0;
  }
  td {
    padding: 5px 8px;
    border-bottom: 1px solid #e2e8f0;
    color: #334155;
  }
  pre {
    background-color: #f8fafc;
    border: 1px solid #e2e8f0;
    border-radius: 6px;
    padding: 6px 10px;
    font-size: 13.5px;
    line-height: 1.25;
    color: #0f172a;
  }
  code {
    font-family: 'Consolas', 'Fira Code', 'Courier New', monospace;
    color: #0f172a;
  }
  header {
    font-size: 11px;
    color: #94a3b8;
    font-weight: 500;
  }
  footer {
    font-size: 11px;
    color: #94a3b8;
  }
---

<!-- SLIDE 0: TITLE SLIDE -->
# MODULAR MONOLITHIC
## Tư duy Thiết kế & Con đường Tiến hóa lên Microservices

**Học phần:** Phát triển Hệ thống Server Nâng cao — Buổi 12  
**Đơn vị:** Bộ môn Kỹ thuật Phần mềm — Khoa Công nghệ Thông tin

<div class="highlight" style="margin-top: 15px;">
<b>Phân công Nghiên cứu & Thuyết trình Chuyên đề:</b>
<br>• <b>Thành viên 1 (TV1):</b> Tổng quan Kiến trúc, Tiêu chí Lựa chọn & Lộ trình 4 Bước Tiến hóa
<br>• <b>Thành viên 2 (TV2):</b> Tư duy Phân rã Nghiệp vụ (DDD), Bounded Context & Thiết kế 10 Module
<br>• <b>Thành viên 3 (TV3):</b> Huyết mạch Giao tiếp Đồng bộ (gRPC), Bất đồng bộ (RabbitMQ) & Tổng kết
</div>

<!-- Speaker Notes:
Kính chào Thầy và toàn thể các bạn sinh viên. Hôm nay nhóm nghiên cứu lý thuyết Buổi 12 xin trân trọng báo cáo chuyên đề: "Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservices".
Trong phát triển hệ thống server hiện đại, kiến trúc phần mềm không đơn thuần là cách chúng ta viết code, mà là sự phản ánh chiến lược kinh doanh, năng lực vận hành và khả năng thích ứng với quy mô tăng trưởng của doanh nghiệp.
Bài báo cáo hôm nay được cấu trúc khoa học theo thế "Kiềng 3 chân" vững chắc:
- TV1 mở đầu với bức tranh vĩ mô: So sánh Monolith, Microservices và Modular Monolith, định luật Conway và lộ trình 4 bước tiến hóa.
- TV2 dẫn dắt vào trọng tâm: Phương pháp luận Domain-Driven Design, Bounded Context, bộ 4 câu hỏi vàng phân rã 10 module E-commerce và kỷ luật cô lập dữ liệu.
- TV3 làm sáng tỏ cơ chế giao tiếp: Đồng bộ qua gRPC và Bất đồng bộ qua RabbitMQ Pub/Sub bảo toàn tin cậy 100%.
-->

---

<!-- SLIDE 1: MONOLITH VS MICROSERVICES -->
# 1. Đặt Vấn Đề: Cuộc Đối Đầu Giữa Hai Thái Cực

<div class="grid-2">
<div>

### 🏢 Monolithic Truyền Thống
* **Bản chất**: 1 Codebase, 1 Process, 1 CSDL chung.
* **Ưu điểm**:
  * Đơn giản, nhấn `F5` chạy ngay trên máy local.
  * Hiệu năng cao: Lời gọi hàm RAM (nanoseconds).
  * Bảo toàn ACID Transactions rất dễ dàng.
* **Hiểm họa khi tăng trưởng**:
  * **Spaghetti Code / Big Ball of Mud** sau vài năm.
  * **Single Point of Failure (SPOF)**: 1 bug kéo sập app.
  * Xung đột merge code; không thể scale độc lập.

</div>
<div>

### 🌐 Microservices Architecture
* **Bản chất**: Đa Repo, Đa Process, DB-per-Service.
* **Lợi ích chiến lược**:
  * Scale co giãn độc lập từng dịch vụ chịu tải.
  * Cô lập vùng lỗi: 1 service sập, hệ thống vẫn chạy.
  * Trao quyền tự chủ cho các Two-Pizza Teams.
* **Khoản thuế phân tán (Distributed Tax)**:
  * Vận hành cực kỳ phức tạp (K8s, Gateway, Tracing).
  * Độ trễ mạng & rủi ro rớt gói tin.
  * Mất ACID $\rightarrow$ Phải dùng Saga & Eventual Consistency.

</div>
</div>

<div class="warning-box">
⚠️ <b>Cảnh báo thực tế:</b> Đa số dự án thất bại không phải vì thiếu công nghệ, mà vì nhảy vào Microservices khi chưa đủ năng lực DevOps và chưa hiểu sâu ranh giới nghiệp vụ!
</div>

<!-- Speaker Notes:
Mở đầu bài thuyết trình, chúng ta nhìn lại hai thái cực kiến trúc phổ biến nhất hiện nay.
Một bên là Monolithic truyền thống—nơi mọi tính năng được nhồi chung vào một tiến trình thực thi và một CSDL chung. Nó rất nhanh và tiện ở giai đoạn đầu, nhưng khi quy mô phình to, nó trở thành "đống bùn lầy khổng lồ", các lập trình viên bắt đầu sửa code giẫm chân lên nhau.
Ở thái cực ngược lại là Microservices—miền đất hứa nơi mọi thứ được chia nhỏ. Nhưng cái giá phải trả là "khoản thuế hệ thống phân tán": chi phí hạ tầng tăng vọt, mất tính nhất quán ACID, và gánh nặng vận hành đè nặng lên vai đội ngũ kỹ sư.
Câu hỏi đặt ra là: Liệu có một con đường dung hòa ở giữa?
-->

---

<!-- SLIDE 2: MODULAR MONOLITHIC SWEET SPOT (LÝ THUYẾT) -->
# 2. Modular Monolithic: Điểm Cân Bằng Tối Ưu
### Sự dung hòa giữa Vận hành đơn khối và Kỷ luật vi dịch vụ

<div class="highlight">
<b>Định nghĩa cốt lõi:</b> Vận hành như một Monolith đơn khối (1 Process duy nhất), nhưng thiết kế mã nguồn với kỷ luật ranh giới cô lập nghiêm ngặt như Microservices (Domain-Centric Modules).
</div>

* **Bốn (4) Trụ Cột Kỹ Thuật Cốt Lõi:**
  1. **Vận hành (Operational):** 1 Repository, 1 Runtime Process, 1 CI/CD Pipeline $\rightarrow$ Chi phí hạ tầng và vận hành tối thiểu.
  2. **Kiến trúc (Architectural):** Phân rã thành các **Bounded Context** độc lập theo chuẩn Domain-Driven Design (DDD).
  3. **Kỷ luật Dữ liệu:** Mỗi module sở hữu Schema riêng, **CẤM TUYỆT ĐỐI SQL JOIN xuyên Schema**.
  4. **Giao tiếp Nội bộ:** Hoàn toàn qua **Public Service Interface** trong bộ nhớ $\rightarrow$ Zero Network Latency!

<div class="highlight" style="margin-top: 10px;">
👉 <b>"Đường lui chiến lược":</b> Đã chuẩn bị sẵn các "đường cắt module"; khi cần scale chỉ việc bóc tách ra Microservice độc lập mà không cần viết lại mã nghiệp vụ!
</div>

<!-- Speaker Notes:
Modular Monolithic xuất hiện như một "điểm ngọt ngào" cứu cánh cho các doanh nghiệp hiện đại.
Về mặt vận hành, nó vẫn là 1 file build duy nhất, chạy trong 1 tiến trình duy nhất. Một lập trình viên mới vào công ty chỉ cần clone repo về và ấn F5 là chạy được ngay, không cần dựng hàng chục Docker container phức tạp.
Nhưng về mặt tổ chức code, các module được đóng gói độc lập theo Bounded Context của DDD. Module Order không được tự tiện thò tay vào bảng dữ liệu của Inventory. Tất cả phải đi qua interface trừu tượng trong RAM.
Nhờ đó, chúng ta vừa có chi phí vận hành siêu rẻ của Monolith, vừa có kỷ luật phân chia mã nguồn tuyệt vời của Microservices.
-->

---

<!-- SLIDE 3: MODULAR MONOLITHIC RUNTIME (SƠ ĐỒ BỘ NHỚ) -->
# 3. Modular Monolithic: Kiến Trúc Thực Thi Trong Bộ Nhớ
### Cơ chế in-process call và cô lập Schema trên máy chủ CSDL

```
+-------------------------------------------------------------------------+
|                        MODULAR MONOLITH RUNTIME                         |
|                       (Single Operating Process)                        |
|                                                                         |
|   [ ORDER MODULE ]             In-Memory Call           [ INVENTORY ]   |
|   - Order.Domain            ======================>     - StockItem     |
|   - Order.Application         (IInventoryService)       - CheckStock    |
|   - Internal DbContext                                  - Internal Db   |
+-------------------------------------------------------------------------+
            |                                                  |
     (order.Orders)                                     (inventory.Stocks)
            v                                                  v
+-------------------------------------------------------------------------+
|                     SINGLE PHYSICAL DATABASE SERVER                     |
|            (Dùng chung 1 DB Server nhưng TÁCH BIỆT SCHEMA)              |
|        - Schema 'order': Chứa bảng Orders, OrderItems (Snapshot)        |
|        - Schema 'inventory': Chứa bảng Stocks, Warehouses               |
|        - Schema 'notification': Chứa bảng NotificationLogs              |
+-------------------------------------------------------------------------+
```

* **Điểm then chốt:** Lời gọi `IInventoryService` diễn ra ngay trong RAM CPU (nanoseconds), không tốn chi phí đóng gói JSON, không tốn kết nối mạng TCP.

<!-- Speaker Notes:
Đây là sơ đồ trực quan nhất về mô hình thực thi của Modular Monolith:
Cả Order Module và Inventory Module cùng nằm trong 1 Process bộ nhớ. Khi Order cần kiểm tra tồn kho, nó gọi trực tiếp qua interface IInventoryService. Lời gọi này diễn ra trong bộ nhớ RAM ở cấp độ nano-giây, hoàn toàn không có độ trễ mạng hay rớt gói tin!
Ở tầng dưới, cả 2 module có thể cùng lưu trữ trên 1 máy chủ PostgreSQL hay SQL Server duy nhất để tiết kiệm chi phí, NHƯNG bảng của Order nằm ở schema "order", còn bảng của Inventory nằm ở schema "inventory". Tuyệt đối không có bất kỳ câu lệnh SQL JOIN nào giữa 2 schema này!
-->

---

<!-- SLIDE 4: BỘ 3 TIÊU CHÍ LỰA CHỌN THỰC TẾ -->
# 4. Bộ 3 Tiêu Chí Lựa Chọn Kiến Trúc Thực Tế
### Đưa ra quyết định dựa trên thực lực doanh nghiệp, không theo trào lưu

<div class="grid-3">
<div>

### 👥 1. Quy Mô Đội Ngũ
*(Conway's Law)*
* **Dưới 15–20 kỹ sư:** Monolithic hoặc Modular Monolith là tối ưu nhất.
* **Nhiều Team độc lập (> 50 người):** Mỗi team làm chủ một miền nghiệp vụ riêng $\rightarrow$ Microservices giúp tránh xung đột triển khai.

</div>
<div>

### 🛠️ 2. Hạ Tầng & DevOps
*(Vận Hành & Chi Phí)*
* **Thiếu DevOps chuyên sâu:** Microservices sẽ biến thành "cơn ác mộng" vận hành.
* **Modular Monolith:** 1 pipeline CI/CD, 1 file đóng gói, giám sát tập trung, tối thiểu hóa chi phí máy chủ.

</div>
<div>

### ⏱️ 3. Giai Đoạn Sản Phẩm
*(Time-to-Market)*
* **MVP / Sản phẩm mới:** Ranh giới nghiệp vụ liên tục biến động $\rightarrow$ Modular Monolith giúp tái cấu trúc cực nhanh.
* **Sản phẩm trưởng thành:** Ranh giới đã ổn định, điểm nghẽn chịu tải rõ ràng $\rightarrow$ Bóc tách đúng điểm nghẽn.

</div>
</div>

<div class="highlight" style="margin-top: 10px;">
💡 <b>Định luật Conway (Conway's Law):</b> Kiến trúc hệ thống luôn phản ánh cơ cấu giao tiếp của tổ chức. Đừng bao giờ xây dựng kiến trúc phân tán nếu đội ngũ của bạn chưa phân tán!
</div>

<!-- Speaker Notes:
Làm thế nào để chọn đúng kiến trúc mà không bị sa vào bẫy trào lưu? Chúng ta căn cứ vào 3 tiêu chí vàng:
Thứ nhất là Quy mô đội ngũ và Định luật Conway: Nếu nhóm chỉ có vài người, việc chia thành 10 microservices là tự sát. Chỉ khi công ty có nhiều nhóm độc lập, mỗi nhóm cần deploy riêng biệt mà không phụ thuộc nhau, Microservices mới phát huy tác dụng.
Thứ hai là Hạ tầng và DevOps: Microservices đòi hỏi Kubernetes, Service Mesh, Tracing tập trung. Nếu thiếu nguồn lực này, Modular Monolith là lựa chọn cứu cánh.
Thứ ba là Giai đoạn sản phẩm: Khi làm MVP, mô hình nghiệp vụ thay đổi hàng tuần. Đổi code trong monolith chỉ mất 5 phút refactor, trong khi đổi API qua mạng giữa 10 microservices mất hàng tuần lễ!
-->

---

<!-- SLIDE 5: MONOLITHFIRST & SHOPIFY -->
# 5. Chiến Lược "MonolithFirst" & Bài Học Thực Chiến Shopify
### Tại sao các tập đoàn hàng tỷ USD kiên quyết gắn bó với Modular Monolith?

* **Nguyên Lý "MonolithFirst" (Martin Fowler - 2015):**
  * Hầu như tất cả hệ thống Microservices thành công đều khởi đầu từ một Monolith được thiết kế tốt.
  * Các dự án khởi đầu trực tiếp bằng Microservices phần lớn rơi vào hỗn loạn và thất bại vì **chưa ai hiểu trọn vẹn ranh giới nghiệp vụ (Boundaries) ngay từ ngày đầu tiên**.

<div class="highlight">
🏢 <b>Case Study Kinh Điển từ Shopify (Shopify Engineering):</b>
<br>• Xử lý hàng trăm tỷ USD doanh thu TMĐT toàn cầu với hơn <b>2.8 triệu dòng code</b>.
<br>• Đứng trước nguy cơ Spaghetti Code, Shopify <b>từ chối đập bỏ để lên Microservices</b>.
<br>• Họ lựa chọn tái cấu trúc thành <b>Modular Monolith khổng lồ</b> (dùng công cụ Packwerk để khóa ranh giới module).
<br>• <b>Kết quả:</b> Tiết kiệm hàng chục triệu USD hạ tầng, thông lượng giao dịch đỉnh cao trong đợt Black Friday!
</div>

* **Minh chứng khác:** **Stack Overflow** (hơn 100M lập trình viên chỉ với vài máy chủ Monolith), **Basecamp / 37signals** (rời bỏ Microservices và Cloud để quay về Monolith on-premise).

<!-- Speaker Notes:
Martin Fowler đã đúc kết nguyên lý MonolithFirst: Hãy luôn bắt đầu với Monolith! Chỉ khi nào hệ thống đủ lớn và có bằng chứng thực tế rõ ràng về sự nghẽn tải, ta mới tính đến chuyện bóc tách.
Minh chứng sống động nhất cho tư duy này chính là Shopify. Với một hệ thống khổng lồ xử lý hàng trăm nghìn đơn hàng mỗi giây, họ từng đối mặt với nguy cơ sụp đổ do code monolith bị rối rắm. Nhưng thay vì chia nhỏ thành hàng trăm microservices, các kỹ sư Shopify quyết định tái cấu trúc thành một Modular Monolith nghiêm ngặt. Họ xây dựng công cụ Packwerk để cấm các module gọi chéo nhau.
Nhờ đó, Shopify vừa duy trì tốc độ xử lý siêu tốc trong bộ nhớ, vừa bảo vệ tính mô-đun hóa mà không phải trả khoản thuế phân tán khổng lồ.
-->

---

<!-- SLIDE 6: MÔ HÌNH KIẾN TRÚC LAI (HYBRID) -->
# 6. Kiến Trúc Lai (Hybrid Architecture): Phá Vỡ Tư Duy Nhị Nguyên
### Giải pháp tối ưu: Giữ khối lõi Monolith và chỉ tách các vệ tinh đặc thù

```
+-------------------------------------------------------------------------+
|                        THE CORE MODULAR MONOLITH                        |
|                  (Duy trì 80% - 90% nghiệp vụ cốt lõi)                  |
|          - Catalog Module                 - Cart Module                 |
|          - Order Processing (Lõi)         - Customer Identity           |
|          - Notification Module            - Review & Rating             |
+-------------------------------------------------------------------------+
           |                                             |
           | Lời gọi Mạng (gRPC, trễ < 5ms)               | Lời gọi Mạng (REST / HTTPS)
           v                                             v
+------------------------------------+        +------------------------------------+
|         INVENTORY SERVICE          |        |          PAYMENT SERVICE           |
|          (Microservice 1)          |        |          (Microservice 2)          |
|  * Chịu tải cực cao Flash Sale     |        |  * Đặt trong mạng DMZ bảo mật cao  |
|  * Scale độc lập 2 -> 20 Pods      |        |  * Đạt chứng chỉ tài chính PCI-DSS |
+------------------------------------+        +------------------------------------+
```

* **Phá vỡ định kiến:** Không bao giờ có chuyện "phải chuyển đổi 100% sang Microservices".
* **Giá trị thực tiễn:** Tối ưu hóa chi phí: Module nào cần scale hoặc cần bảo mật đặc biệt mới tách ra; toàn bộ phần còn lại vẫn vận hành thanh thoát bên trong khối lõi!

<!-- Speaker Notes:
Một quan niệm rất sai lầm là: Đã làm Microservices thì toàn bộ hệ thống phải là Microservices.
Thực tế trong các doanh nghiệp lớn áp dụng mô hình Kiến trúc Lai (Hybrid Architecture): Khối lõi Modular Monolith vẫn nắm giữ 80 đến 90% nghiệp vụ như Quản lý sản phẩm, Giỏ hàng, Thông báo, Đánh giá.
Họ chỉ tách đúng 1 hoặc 2 module có lý do kinh doanh sống còn:
Thứ nhất: Module Tồn kho (Inventory) được tách ra thành microservice vì đợt Flash Sale 11.11 có hàng triệu người cùng vào check kho, nó cần scale co giãn từ 2 pod lên 20 pod độc lập.
Thứ hai: Module Thanh toán (Payment) được tách ra để đặt trong vùng mạng cô lập nhằm đạt chứng chỉ an ninh tài chính PCI-DSS, giúp tiết kiệm chi phí kiểm toán cho toàn bộ công ty.
-->

---

<!-- SLIDE 7: LỘ TRÌNH 4 BƯỚC TIẾN HÓA (CHI TIẾT BƯỚC) -->
# 7. Lộ Trình 4 Bước Tiến Hóa Tách Module: Chi Tiết Từng Bước
### Nguyên tắc: Giữ nguyên 100% Core Logic nhờ tuân thủ Modular Monolith

<div class="highlight">
<b>Nguyên tắc vàng:</b> Nhờ ranh giới Public Interface đã xác lập từ đầu, <b>100% Core Domain & Application Logic được giữ nguyên vẹn</b>, tuyệt đối không cần viết lại mã nghiệp vụ!
</div>

* **Bước 1: Tách Database Vật Lý Riêng Biệt**
  * Di chuyển Schema của module sang Database Server độc lập.
  * Triệt tiêu hoàn toàn Khóa ngoại (FK) vật lý; cấp Connection String riêng.
* **Bước 2: Chuyển Đổi Interface Sang Lời Gọi Mạng**
  * Microservice mới: Bổ sung Web API / gRPC Controller mỏng nhận request.
  * Khối lõi Monolith: Thay Adapter in-process bằng gRPC Client cùng Interface.
  * Tích hợp thư viện phòng vệ **Polly** (Timeout 2s, Retry, Circuit Breaker).
* **Bước 3: Đóng Gói Container Docker Độc Lập** *(Kế thừa Buổi 6, 7)*
  * Viết Dockerfile Multi-stage build tối ưu kích thước (< 150MB).
* **Bước 4: Thiết Lập Pipeline CI/CD Riêng Biệt** *(Kế thừa Buổi 10, 11)*
  * Path-filtering trigger tự động test, scan bảo mật và zero-downtime deploy.

<!-- Speaker Notes:
Làm thế nào để tách một module ra thành Microservice mà không làm gián đoạn hệ thống và không phải đập đi viết lại code?
Nhờ Modular Monolith đã xây dựng chuẩn, chúng ta chỉ cần thực hiện đúng 4 bước:
Bước 1: Tách CSDL vật lý. Vì dữ liệu đã nằm riêng ở Schema từ trước, ta chỉ việc backup và restore sang cụm server mới, xóa khóa ngoại cứng.
Bước 2: Thay đường truyền in-process bằng network call. Vì OrderService chỉ phụ thuộc vào interface IInventoryService, nên phía Order ta chỉ việc thay class InProcessInventoryService bằng GrpcInventoryClient. Code nghiệp vụ của Order không đổi một chữ nào!
Bước 3: Đóng gói Dockerfile Multi-stage build theo kiến thức Buổi 6, 7.
Bước 4: Tạo pipeline CI/CD riêng biệt theo kiến thức Buổi 10, 11.
-->

---

<!-- SLIDE 8: LỘ TRÌNH 4 BƯỚC (SƠ ĐỒ & CẠM BẪY) -->
# 8. Lộ Trình 4 Bước: Sơ Đồ Quy Trình & Cạm Bẫy Sống Còn
### Cảnh giác với nguy cơ thoái hóa thành "Distributed Monolith"

```
     [ BƯỚC 1: TÁCH DATABASE VẬT LÝ ]
        - Di chuyển Schema sang DB Server riêng biệt
        - Triệt tiêu khóa ngoại FK cứng -> Thay bằng Logical Reference
                    |
                    v
     [ BƯỚC 2: INTERFACE -> LỜI GỌI MẠNG ]
        - Expose gRPC / REST Controller tại Microservice mới
        - Khối lõi thay Adapter in-process bằng gRPC Client + Polly
                    |
                    v
     [ BƯỚC 3: ĐÓNG GÓI DOCKER CONTAINER ]
        - Multi-stage build Dockerfile (ASP.NET Alpine Runtime < 150MB)
                    |
                    v
     [ BƯỚC 4: PIPELINE CI/CD RIÊNG BIỆT ]
        - Path-based trigger: 'src/Modules/Inventory/**'
        - Release độc lập không phụ thuộc vào chu kỳ Monolith
```

<div class="warning-box">
⚠️ <b>Cạm bẫy chết người:</b> Nếu Bước 1 bị vi phạm (vẫn còn thói quen viết SQL JOIN chéo bảng qua mạng hoặc dùng chung DB), hệ thống sẽ lập tức biến thành <b>Distributed Monolith</b>—thảm họa tồi tệ nhất trong kỹ nghệ phần mềm!
</div>

<!-- Speaker Notes:
Sơ đồ 4 bước này là kim chỉ nam cho mọi kỹ sư khi muốn phân rã hệ thống.
Nhưng hãy hết sức lưu ý chiếc hộp màu vàng cảnh báo ở đây: Cạm bẫy lớn nhất là Distributed Monolith.
Nếu ở bước 1, bạn không triệt tiêu triệt để các câu lệnh SQL JOIN chéo, mà để service A kết nối trực tiếp vào database của service B qua mạng, bạn sẽ gánh chịu toàn bộ nhược điểm của cả hai mô hình: vừa chịu độ trễ mạng chập chờn của Microservices, vừa bị phụ thuộc chặt cứng database của Monolith!
-->

---

<!-- SLIDE 9: CASE STUDY TÁCH INVENTORY (CODE ĐỐI CHIẾU) -->
# 9. Bóc Tách Module Inventory: Mã Nguồn Trước & Sau
### Minh chứng thực tế: Logic nghiệp vụ hoàn toàn không thay đổi!

<div class="grid-2">
<div>

### 📦 Trước Khi Tách (Modular Monolith)
* `OrderService` gọi trực tiếp interface trong RAM:
  ```csharp
  // Đăng ký in-process trong Startup/Program:
  services.AddScoped<IInventoryService, 
      InProcessInventoryService>();

  // OrderService gọi bình thường:
  var isOk = await _inventoryService
      .CheckStockAsync(productId, qty);
  ```
* Bảng `inventory.Stocks` nằm chung máy chủ DB với `order.Orders`.
* Rủi ro: Flash Sale nghẽn Connection Pool DB chung.

</div>
<div>

### 🚀 Sau Khi Tách (Microservice Độc Lập)
* **Giữ nguyên 100%**: `Inventory.Domain` & `Application`.
* Đăng ký DI phía Order bằng Network Adapter:
  ```csharp
  // Thay thế class triển khai DI Container:
  services.AddHttpClient<IInventoryService, 
      InventoryHttpServiceClient>(c => {
      c.BaseAddress = new Uri("https://inventory-svc");
  });
  ```
* Tích hợp thư viện phòng vệ **Polly**:
  * Timeout 2s $\rightarrow$ Chặn đứng treo luồng.
  * Circuit Breaker $\rightarrow$ Ngắt mạch khi lỗi > 50%.

</div>
</div>

<div class="highlight" style="margin-top: 10px;">
✔ <b>Kết luận:</b> Mã nguồn trong `OrderService.cs` <b>không thay đổi dù chỉ một dòng</b> nhờ triết lý Dependency Inversion!
</div>

<!-- Speaker Notes:
Hãy nhìn vào hai đoạn code đối chiếu này—đây chính là bằng chứng thuyết phục nhất trước hội đồng!
Ở bên trái, khi còn là Modular Monolith, chúng ta inject InProcessInventoryService.
Ở bên phải, khi bóc tách Inventory thành Microservice, class OrderService vẫn giữ nguyên 100% logic nghiệp vụ. Chúng ta chỉ đổi duy nhất cấu hình Dependency Injection sang InventoryHttpServiceClient hoặc GrpcClient.
Toàn bộ mã nghiệp vụ trừ kho, kiểm tra số lượng tồn đều được tái sử dụng nguyên vẹn. Đó chính là sức mạnh của việc thiết kế kiến trúc chuẩn mực ngay từ đầu!
-->

---

<!-- SLIDE 10: CASE STUDY TÁCH INVENTORY (SƠ ĐỒ GỌI MẠNG) -->
# 10. Bóc Tách Module Inventory: Sơ Đồ Kiến Trúc Luồng Gọi
### Chuyển đổi từ gọi trong bộ nhớ sang giao tiếp mạng kiên cố

```
+-------------------------------------------------------------------------+
|                              ORDER MODULE                               |
|        OrderService (Mã nguồn nghiệp vụ Core GIỮ NGUYÊN 100%)           |
|                               |                                         |
|                               v (Thực thi interface IInventoryService)  |
|                 InventoryHttpServiceClient / GrpcClient                 |
|                 (Tích hợp Polly: Timeout 2s, Circuit Breaker)           |
+-------------------------------------------------------------------------+
                                    |
                                    | Lời gọi Mạng (gRPC HTTP/2 / REST)
                                    v
+-------------------------------------------------------------------------+
|                       INVENTORY MICROSERVICE POD                        |
|       InventoryApiController / GrpcEndpoint (Lớp Adapter mỏng mới)       |
|                               |                                         |
|                               v                                         |
|       InventoryApplicationService (Tái sử dụng 100% mã nguồn cũ!)       |
|                               |                                         |
|                               v                                         |
|                 [ Dedicated Inventory Database Server ]                 |
+-------------------------------------------------------------------------+
```

<div class="highlight">
🔗 <b>Bản lề chuyển tiếp:</b> Để có được đường cắt interface hoàn hảo này, <b>Tư duy Phân rã Nghiệp vụ DDD của Thành viên 2</b> ngay sau đây là nền tảng quyết định!
</div>

<!-- Speaker Notes:
Sơ đồ luồng này khép lại phần trình bày của Thành viên 1.
Nhìn vào sơ đồ, chúng ta thấy rõ: Đường biên giới giữa hai hệ thống được bảo vệ bởi một adapter mỏng. Order Service gọi gRPC tới Inventory Pod. Nếu mạng bị gián đoạn, Polly sẽ kích hoạt Circuit Breaker để bảo vệ hệ thống.
Nhưng câu hỏi cốt lõi là: Làm thế nào để chúng ta biết nên cắt module ở đâu? Tại sao lại cắt Inventory mà không gộp chung vào Catalog?
Xin mời Thầy và các bạn cùng đến với Phần 2 của bạn Thành viên 2 về Tư duy phân rã nghiệp vụ Domain-Driven Design!
-->

---

<!-- SLIDE 11: TV2 CẠM BẪY CHIA NGANG -->
# 11. Tư Duy Phân Rã Nghiệp Vụ: Cạm Bẫy Chia Ngang Kỹ Thuật
### Phần trình bày Thành viên 2 (Mục 2.2 — Trọng tâm cốt lõi của buổi học)

<div class="grid-2">
<div>

### ❌ Hai Sai Lầm Phân Chia Kinh Điển
1. **Chia theo cảm tính (Ad-hoc):**
   * Gom cụm chức năng ngẫu nhiên $\rightarrow$ Tạo ra các **God Class** gánh hàng chục trách nhiệm, ai đụng vào cũng sợ sập.
2. **Chia theo lớp kỹ thuật ngang (Horizontal):**
   * 1 folder `Controllers`, 1 folder `Services`, 1 folder `Repositories` dùng chung toàn app.

</div>
<div>

### 💥 Hậu Quả Của Mô Hình Chia Ngang
* **Vi phạm Common Closure Principle (CCP):** 1 yêu cầu nghiệp vụ thay đổi (ví dụ: đổi cách tính thuế) buộc lập trình viên phải sửa rải rác ở hàng loạt tầng khác nhau.
* **Xung đột mã nguồn (Merge Hell):** Hàng chục kỹ sư cùng sửa chung vào 1 DbContext và các entity khổng lồ.
* Không thể bóc tách bất kỳ module nào ra thành microservice sau này!

</div>
</div>

```
            MÔ HÌNH PHÂN CHIA THEO LỚP KỸ THUẬT NGANG (TRUYỀN THỐNG - SAI)
+-------------------------------------------------------------------------------------+
| Controllers Layer  : ProductController, OrderController, CartController,...       |
+-------------------------------------------------------------------------------------+
| Services Layer     : ProductService, OrderService (Gọi chéo nhau tùy tiện, rối mù)  |
+-------------------------------------------------------------------------------------+
| Data / DbContext   : AppDbContext duy nhất, SQL JOIN chéo 20 bảng vô tội vạ         |
+-------------------------------------------------------------------------------------+
```

<!-- Speaker Notes:
Xin kính chào Thầy và các bạn, tôi là Thành viên 2, chịu trách nhiệm phần trọng tâm nhất của buổi học: Tư duy phân rã nghiệp vụ.
Trước khi nói về cách làm đúng, chúng ta hãy nhìn vào sai lầm mà hầu như 90% sinh viên và các lập trình viên mới đi làm đều mắc phải: Phân chia theo lớp kỹ thuật ngang.
Các bạn tạo ra một thư mục Controllers, một thư mục Services, một thư mục Repositories. Kết quả là gì? Một tính năng bán hàng bị xé lẻ ra 4 nơi. Khi nghiệp vụ đổi một chút, bạn phải nhảy qua 4 file để sửa. Các service tự do inject chéo nhau, tạo thành một mớ dây tơ vò không lối thoát.
-->

---

<!-- SLIDE 12: TV2 NGUYÊN LÝ CHIA DỌC (VERTICAL SLICING) -->
# 12. Tư Duy Phân Rã Nghiệp Vụ: Nguyên Lý Vertical Slicing & DDD
### Phân rã bài toán lớn thành các Bounded Context độc lập, khép kín

* **Vertical Slicing (Cắt Lát Theo Chiều Dọc):** Mượn từ triết lý **Domain-Driven Design (DDD)** của Eric Evans.
* Mỗi module là một **Bounded Context** khép kín hoàn chỉnh từ tầng tiếp nhận (API), tầng điều phối nghiệp vụ (Application), tầng nghiệp vụ thuần túy (Domain) cho đến tầng lưu trữ dữ liệu (Infrastructure Schema).

```
          MÔ HÌNH PHÂN RÃ NGHIỆP VỤ THEO CHIỀU DỌC (MODULAR MONOLITH - ĐÚNG)
+-------------------+   +-------------------+   +-------------------+   +-------------------+
|  CATALOG MODULE   |   |   ORDER MODULE    |   | INVENTORY MODULE  |   |  PAYMENT MODULE   |
| - Catalog API     |   | - Order API       |   | - Inventory API   |   | - Payment API     |
| - Domain Logic    |   | - Domain Logic    |   | - Domain Logic    |   | - Domain Logic    |
| - catalog.* Schema|   | - order.* Schema  |   | - inv.* Schema    |   | - payment.* Schema|
+-------------------+   +-------------------+   +-------------------+   +-------------------+
```

<div class="highlight">
🛡️ <b>Ranh giới phòng vệ (Encapsulation Boundary):</b> Mô hình dữ liệu bên trong module được bảo vệ tuyệt đối. Module bên ngoài chỉ được nhìn thấy Interface công khai, không bao giờ được phép chạm vào chi tiết lưu trữ nội bộ!
</div>

<!-- Speaker Notes:
Để giải quyết tận gốc vấn đề, chúng ta phải chuyển dịch tư duy sang Vertical Slicing—cắt lát theo chiều dọc dựa trên Domain-Driven Design.
Mỗi bài toán nghiệp vụ lớn được chia thành các Bounded Context nhỏ khép kín. Module Catalog chịu trách nhiệm từ hiển thị đến lưu trữ sản phẩm; Module Order chịu trách nhiệm trọn vẹn vòng đời đơn hàng; Module Inventory chịu trách nhiệm toàn bộ việc xuất nhập kho.
Mỗi module là một "vương quốc độc lập", có đường biên giới phòng vệ kiên cố. Nhờ đó, chúng ta triệt tiêu hoàn toàn sự rò rỉ logic và sẵn sàng cho mọi kịch bản mở rộng sau này.
-->

---

<!-- SLIDE 13: BOUNDED CONTEXT: ĐA NGHĨA CỦA "PRODUCT" -->
# 13. Bounded Context: Sự Đa Nghĩa Của Thực Thể "Product"
### Ảo tưởng về "Canonical Data Model" dùng chung cho toàn doanh nghiệp

<div class="highlight">
<b>Quy tắc vàng DDD:</b> Một thuật ngữ chỉ có ý nghĩa chính xác, duy nhất bên trong một Bounded Context cụ thể. Gom chung tất cả vào 1 class sẽ tạo ra một <b>God Entity</b> biến dạng với 60–70 thuộc tính!
</div>

* **Sự Đa Nghĩa Của "Product" Qua 4 Ngữ Cảnh E-Commerce:**
  1. **Catalog Context (Sản phẩm chào bán):** Title, CategoryId, ImageGallery, SEO Description, BasePrice $\rightarrow$ *Tối ưu tìm kiếm và giao diện đẹp mắt.*
  2. **Inventory Context (Đơn vị lưu kho SKU):** SKU Code, Barcode, Kệ kho Bin/Aisle, Tồn khả dụng (ATP) $\rightarrow$ *Kiểm soát chính xác số lượng thực tế trong kho.*
  3. **Shipping Context (Kiện hàng vận chuyển):** Khối lượng (gram), Kích thước Dài-Rộng-Cao, Hàng dễ vỡ $\rightarrow$ *Tính cước phí giao vận xe tải.*
  4. **Order Context (Dòng mục đơn hàng):** **Bản chụp bất biến (Immutable Snapshot)** gồm tên và giá tại thời điểm bấm mua $\rightarrow$ *Ràng buộc pháp lý và hóa đơn kế toán.*

<div class="warning-box">
⚠️ <b>Tính bất biến sống còn:</b> Nếu ngày mai Catalog đổi tên sản phẩm hoặc tăng giá từ 20tr lên 25tr, đơn hàng khách đã mua hôm qua <b>tuyệt đối không được phép đổi theo</b>!
</div>

<!-- Speaker Notes:
Khái niệm cốt lõi nhất của DDD là Bounded Context, và minh chứng rõ ràng nhất chính là từ "Product" (Sản phẩm).
Nếu một lập trình viên non tay thiết kế CSDL, bạn sẽ tạo ra một bảng Product duy nhất chứa từ tên, ảnh, mô tả, cho đến vị trí kệ kho, cân nặng vận chuyển, giá bán quá khứ... Class đó sẽ phình to thành một con quái vật 70 cột!
Trong thực tế, ở Catalog, Product là món hàng chào bán đẹp mắt. Nhưng ở Inventory, nó chỉ là một mã SKU nằm ở kệ số mấy. Ở Shipping, nó là một kiện hàng nặng bao nhiêu gram để xếp lên xe tải.
Và đặc biệt nhất: Ở Order, nó là một Snapshot bất biến. Nếu tuần sau shop tăng giá sản phẩm, đơn hàng đã chốt của khách hàng hôm nay TUYỆT ĐỐI không được biến động theo!
-->

---

<!-- SLIDE 14: UBIQUITOUS LANGUAGE: SỰ CHUYỂN HÓA CỦA "ORDER" -->
# 14. Ubiquitous Language: Sự Chuyển Hóa Của Thực Thể "Order"
### Thống nhất ngôn ngữ chuyên ngành và theo dõi vòng đời giao dịch

* **Ngôn Ngữ Chung (Ubiquitous Language):** Là từ vựng nghiêm ngặt được đồng thuận tuyệt đối giữa Chuyên gia nghiệp vụ (Domain Expert) và Kỹ sư phần mềm.
* **Vòng Đời Chuyển Hóa Của "Order" Qua Các Bounded Context:**

```
[ CART CONTEXT ]          [ ORDER CONTEXT ]          [ PAYMENT CONTEXT ]        [ SHIPPING CONTEXT ]
 Shopping Cart             Legal Sales Order          Payment Transaction        Waybill / Consignment
- Dữ liệu tạm thời        - Hợp đồng pháp lý         - Giao dịch thu tiền       - Vận đơn giao nhận
- Tỷ lệ bỏ rơi 70%        - Status: Confirmed        - Gateway Ref, Mã thẻ      - Tracking Number
- Lưu trên Redis RAM      - ACID Database            - Hoàn tiền (Refund)       - Shipper / 3PL
```

<div class="highlight">
💡 <b>Bài học kiến trúc:</b> Thay vì ép buộc một class duy nhất mang tên `Order`, việc mô hình hóa thành 4 thực thể chuyên biệt giúp hệ thống vận hành cực kỳ thanh thoát, đúng vai trò và loại bỏ hoàn toàn các trường dữ liệu bị `null` vô nghĩa!
</div>

<!-- Speaker Notes:
Bên cạnh Product, chúng ta hãy nhìn vào thực thể "Order" để thấy sức mạnh của Ubiquitous Language.
Khi người dùng đang chọn đồ, đó là "Shopping Cart"—dữ liệu tạm thời nằm trên Redis cache, khách bỏ rơi thì xóa sau 7 ngày.
Khi người dùng bấm Đặt hàng, nó chuyển thành "Sales Order"—hợp đồng mua bán có giá trị pháp lý, được bảo vệ bằng giao dịch ACID.
Sang cổng thanh toán, nó là "Payment Transaction"—chỉ quan tâm đến mã đối soát ngân hàng và trạng thái trừ tiền.
Và sang bưu cục giao vận, nó biến thành "Waybill / Vận đơn"—chỉ quan tâm đến địa chỉ giao nhận và mã tracking của shipper.
Tách biệt rõ ràng 4 khái niệm này giúp code của bạn trong sáng, dễ bảo trì và phản ánh chính xác thế giới thực.
-->

---

<!-- SLIDE 15: BỘ 4 CÂU HỎI VÀNG PHÂN ĐỊNH RANH GIỚI -->
# 15. Bộ 4 Câu Hỏi Vàng Xác Định Ranh Giới Module
### Công cụ tư duy khách quan, khoa học — Loại bỏ hoàn toàn sự cảm tính

<div class="highlight">
Để phân định ranh giới một module có hợp lý hay không, kiến trúc sư áp dụng <b>Bộ 4 Câu Hỏi Vàng</b>:
</div>

| # | Câu hỏi vàng | Bản chất kỹ thuật & Nghiệp vụ | Mục tiêu bảo vệ |
|---|---|---|---|
| **1** | **Ngôn ngữ nghiệp vụ riêng?** | Nghiệp vụ có hệ thống thuật ngữ, khái niệm đặc thù không? | Chống ép khuôn mô hình (Model Smashing) |
| **2** | **Ranh giới tổ chức / phòng ban?** | Phòng ban nào trong thực tế chịu trách nhiệm KPI và sở hữu? | Đồng quy Định luật Conway, xác định Product Owner |
| **3** | **Độc lập dữ liệu (No-JOIN)?** | Dữ liệu có thể lưu riêng mà KHÔNG CẦN viết SQL JOIN chéo? | Triệt tiêu phụ thuộc CSDL, sẵn sàng bóc tách |
| **4** | **Tốc độ thay đổi yêu cầu?** | Tần suất thay đổi nhanh (theo tuần) hay ổn định (theo năm)? | Cô lập phạm vi ảnh hưởng lỗi (Blast Radius) |

<!-- Speaker Notes:
Để không bị rơi vào tranh cãi cảm tính giữa các kỹ sư xem nên chia hệ thống thành 5 hay 10 module, tài liệu môn học Buổi 12 trang bị cho chúng ta Bộ 4 câu hỏi vàng:
1. Ngôn ngữ có riêng không? Nếu dùng chung từ ngữ nhưng nghĩa khác nhau -> Phải tách.
2. Phòng ban nào sở hữu? Marketing quản lý Promotion; Kế toán quản lý Payment; Thủ kho quản lý Inventory. Ranh giới tổ chức chính là ranh giới module.
3. Dữ liệu có đứng độc lập không? Nếu bắt buộc phải viết câu lệnh JOIN giữa 2 bảng thì ranh giới có thể đang sai.
4. Tốc độ thay đổi thế nào? Khuyến mãi đổi liên tục theo chiến dịch tuần; trong khi Danh mục sản phẩm cả năm mới đổi cấu trúc một lần. Tách riêng để vùng biến động cao không làm rung chuyển vùng ổn định.
-->

---

<!-- SLIDE 16: MA TRẬN 10 NGHIỆP VỤ (PHẦN 1) -->
# 16. Ma Trận 10 Nghiệp Vụ E-Commerce (Phần 1: Module 1 - 5)
### Ứng dụng Bộ 4 câu hỏi vàng đánh giá chi tiết từng module

| STT | Nghiệp vụ | 1. Ngôn ngữ riêng | 2. Phòng ban | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định |
|:---:|---|---|---|---|:---:|:---:|
| **1** | **Catalog** | Category, Specs, DisplayPrice | Merchandising & Media | Độc lập cao: Lưu thông tin mô tả, ảnh; cấm JOIN bảng kho/đơn | Thấp - TB | **TÁCH RIÊNG** |
| **2** | **Cart** | ShoppingCart, CartItem, Subtotal | Growth & UX Team | Độc lập cao: Lưu Redis/Schema riêng; chỉ lưu ProductId, Qty | Trung bình | **TÁCH RIÊNG** |
| **3** | **Order** | SalesOrder, OrderLineItem, Snapshot | Vận hành & Kế toán | Độc lập cao: Lưu **Snapshot** sản phẩm; cấm JOIN Catalog | Thấp | **TÁCH LÕI** |
| **4** | **Payment** | Transaction, Gateway, Settlement | Tài chính & Kế toán | Tuyệt đối: Lưu log đối soát ngân hàng; chỉ lưu OrderId | Thấp (PCI-DSS) | **TÁCH RIÊNG** |
| **5** | **Inventory**| SKU, StockLevel, ReservedStock, ATP | Quản lý Kho & Vận hành | Độc lập cao: Quản lý số lượng tồn theo SKU; tra cứu riêng | Thấp | **TÁCH RIÊNG** |

<!-- Speaker Notes:
Áp dụng Bộ 4 câu hỏi vàng vào bài toán E-commerce thực tế, chúng ta thu được bảng ma trận cho 5 module đầu tiên:
Catalog và Cart tách riêng vì Cart biến động liên tục trên Redis, còn Catalog lưu trữ thông tin sản phẩm tĩnh.
Order là hạt nhân cốt lõi, lưu trữ snapshot bất biến của đơn hàng.
Payment tách riêng vì liên quan đến cổng thanh toán và bảo mật tài chính.
Inventory tách riêng vì vận hành kho cần các khái niệm chuyên sâu như SKU và ATP (Available to Promise).
-->

---

<!-- SLIDE 17: MA TRẬN 10 NGHIỆP VỤ (PHẦN 2) -->
# 17. Ma Trận 10 Nghiệp Vụ E-Commerce (Phần 2: Module 6 - 10)
### Hoàn thiện bức tranh phân rã toàn diện 10 miền nghiệp vụ

| STT | Nghiệp vụ | 1. Ngôn ngữ riêng | 2. Phòng ban | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định |
|:---:|---|---|---|---|:---:|:---:|
| **6** | **Shipping** | Waybill, Consignment, ParcelWeight | Logistics & Đối tác 3PL | Độc lập cao: Lưu địa chỉ giao, cân nặng, mã tracking | Trung bình | **TÁCH RIÊNG** |
| **7** | **Identity** | UserAccount, Role, Claim, JWT Hash | An ninh mạng & IT | Tuyệt đối: Lưu tài khoản, mật khẩu; module khác lưu UserId | Rất thấp | **TÁCH RIÊNG** |
| **8** | **Notification**| Template, Channel (SMS/Email/Push) | Chăm sóc Khách hàng | Tuyệt đối: Nhận payload từ Queue; không lưu data nghiệp vụ | Trung bình | **TÁCH RIÊNG** |
| **9** | **Promotion** | Voucher, Quy tắc giảm giá, Flash Sale | Tiếp thị Chiến dịch | Độc lập cao: Lưu quy tắc voucher; tính tiền giảm qua DTO | **RẤT CAO** | **TÁCH RIÊNG** |
| **10**| **Review** | Rating 1-5 sao, Feedback, Verified | Quản lý Chất lượng | Độc lập cao: Lưu đánh giá theo ProductId và UserId | Thấp | **TÁCH RIÊNG** |

<div class="highlight" style="margin-top: 10px;">
🎯 <b>Đánh giá tổng kết:</b> Cả 10 nghiệp vụ đều thỏa mãn xuất sắc Bộ 4 câu hỏi vàng $\rightarrow$ Việc thiết lập 10 module độc lập là phương án kiến trúc tối ưu nhất!
</div>

<!-- Speaker Notes:
Tiếp tục với 5 module còn lại:
Shipping thuộc về bộ phận giao vận Logistics và các hãng vận chuyển thứ 3.
Identity chịu trách nhiệm xác thực, cấp token JWT bảo mật.
Notification thuần túy nhận tin nhắn từ Queue để bắn Email/SMS, hoàn toàn phi trạng thái.
Đặc biệt, Promotion (Khuyến mãi) có tốc độ thay đổi cực nhanh: Marketing có thể đổi công thức giảm giá mỗi ngày. Tách Promotion ra giúp việc sửa đổi quy tắc khuyến mãi không bao giờ làm ảnh hưởng đến khối Order lõi!
-->

---

<!-- SLIDE 18: CẤU TRÚC CLEAN ARCHITECTURE NỘI BỘ -->
# 18. Cấu Trúc Clean Architecture Nội Bộ Từng Module
### Kế thừa Buổi 4: Mỗi module là một ứng dụng độc lập thu nhỏ

```
src/Modules/Inventory/
├── Domain/                 <-- Nghiệp vụ thuần khiết (Entities, Value Objects, Domain Events)
│   ├── Entities/ (StockItem.cs, Warehouse.cs)  * CẤM dính dáng ORM/EF Core
│   └── ValueObjects/ (SKU.cs, BinLocation.cs)  * CẤM tham chiếu module khác
├── Application/            <-- Use Cases & CQRS điều phối luồng xử lý
│   ├── Commands/ (DeductStockCommand.cs)
│   └── Queries/ (CheckStockAvailabilityQuery.cs)
├── Infrastructure/         <-- CSDL & Persistence NỘI BỘ (Đóng gói chặt chẽ)
│   ├── Persistence/ (InventoryDbContext.cs)    * KHAI BÁO INTERNAL
│   └── Repositories/ (StockItemRepository.cs)
└── Contracts/              <-- CỬA NGÕ CÔNG KHAI DUY NHẤT (Public API)
    ├── IInventoryService.cs                    * PUBLIC INTERFACE
    └── DTOs/ (CheckStockRequestDto.cs, StockCheckResultDto.cs)
```

<div class="highlight">
🔒 <b>Kỷ luật truy cập C#/.NET:</b> Tận dụng triệt để từ khóa <code>internal</code> trong code: <code>internal class InventoryDbContext</code> giúp ngăn chặn triệt để lập trình viên module khác gọi chéo CSDL!
</div>

<!-- Speaker Notes:
Bên trong từng module, chúng ta tổ chức mã nguồn như thế nào?
Chúng ta kế thừa trọn vẹn kiến trúc Clean Architecture đã học ở Buổi 4:
- Tầng Domain chứa logic nghiệp vụ thuần khiết, không dính ORM.
- Tầng Application chứa Use Case điều phối.
- Tầng Infrastructure chứa DbContext nội bộ.
- Tầng Contracts là cửa ngõ công khai duy nhất đưa ra interface và DTO.
Bí quyết thực chiến trong .NET là: Hãy khai báo class InventoryDbContext ở mức độ "internal"! Nhờ đó, trình biên dịch sẽ chặn đứng ngay lập tức nếu một lập trình viên ở module Order cố tình inject DbContext của Inventory!
-->

---

<!-- SLIDE 19: 4 KỶ LUẬT CÔ LẬP DỮ LIỆU THÉP -->
# 19. Bốn (4) Kỷ Luật Cô Lập Dữ Liệu "Thép"
### Giới nghiêm tối cao để bảo vệ kiến trúc Modular Monolith

<div class="grid-2">
<div>

### 🛡️ 4 Điều Luật Giới Nghiêm
1. **Phân tách Schema rõ ràng:**
   * Mỗi module 1 Schema riêng: `catalog.*`, `order.*`, `inventory.*`.
2. **CẤM TUYỆT ĐỐI SQL JOIN xuyên Schema:**
   * Áp dụng **Snapshot Pattern** hoặc gọi qua Public Interface.
3. **CẤM inject chéo DbContext:**
   * Không bao giờ inject repository của module khác vào Use Case.
4. **CẤM tạo Foreign Key (FK) cứng:**
   * Chỉ lưu ID logic (UUID/GUID).

</div>
<div>

### 🤖 Tự Động Hóa Kiểm Soát (CI/CD)
* Không dựa vào ý thức tự giác của lập trình viên!
* Tích hợp thư viện kiểm toán kiến trúc **NetArchTest / ArchUnit** vào Unit Test:
```csharp
// Kiểm tra ranh giới kiến trúc tự động:
Types.InAssembly(orderAssembly)
  .ShouldNot()
  .HaveDependencyOn("Modules.Inventory.Infrastructure")
  .Assert();
```
* Nếu ai đó lỡ tay inject chéo $\rightarrow$ **Build CI/CD lập tức bị FAIL!**

</div>
</div>

<!-- Speaker Notes:
Để giữ cho Modular Monolith không biến chất thành đống rác Spaghetti, nhóm đề ra 4 kỷ luật thép:
1. Schema riêng biệt.
2. Tuyệt đối cấm SQL JOIN chéo bảng giữa 2 schema.
3. Tuyệt đối cấm gọi DbContext chéo.
4. Cấm tạo khóa ngoại cứng vật lý giữa các bảng của module khác nhau.
Đặc biệt, chúng ta không tin vào lời hứa của lập trình viên. Nhóm đã tích hợp thư viện NetArchTest vào quy trình CI/CD. Bất kỳ ai lén lút inject chéo DbContext, Unit Test kiến trúc sẽ kích hoạt và chặn đứng không cho merge code!
-->

---

<!-- SLIDE 20: PUBLIC SERVICE INTERFACE (BẢN LỀ TIẾN HÓA) -->
# 20. Bản Lề Tiến Hóa (Public Interface) & Cạm Bẫy
### "Đường cắt kiến trúc" (Architectural Seam) kết nối các module

* **Interface `IInventoryService` — Đường cắt kiến trúc hoàn hảo (Michael Feathers):**
  * **Hôm nay (Modular Monolith):** Đăng ký thực thi trong RAM (`InProcessInventoryService`), tốc độ xử lý nano-giây.
  * **Ngày mai (Microservice):** Đăng ký thực thi qua mạng (`GrpcInventoryClient`), mã nghiệp vụ Order giữ nguyên 100%!

```
// Order Module chỉ phụ thuộc vào Interface trừu tượng và DTO nguyên thủy:
public interface IInventoryService {
    Task<StockCheckResultDto> CheckStockAsync(CheckStockRequestDto request);
}
```

<div class="warning-box">
⚠️ <b>Cạm bẫy "Shared Code Library":</b> Tránh việc tạo một thư viện <code>Shared.Domain</code> rồi nhét tất cả Entity vào đó! Việc chia sẻ mã nguồn thực thể sẽ kéo chằng chịt các module lại với nhau, làm mất sạch tính độc lập của Bounded Context.
</div>

<!-- Speaker Notes:
Interface công khai chính là chiếc "bản lề" kiến trúc. Khi còn chạy chung tiến trình, interface này trỏ tới code trong bộ nhớ. Khi bóc tách ra microservice, interface này trỏ tới gRPC client.
Và cạm bẫy lớn nhất mà các nhóm hay gặp phải là tạo ra một project "Shared.Domain" rồi ném toàn bộ Entity vào đó để dùng chung. Đó là một sai lầm chết người! Chia sẻ Entity đồng nghĩa với việc xóa nhòa ranh giới Bounded Context. Thứ duy nhất được chia sẻ chỉ là các Interface trừu tượng và Event DTO nguyên thủy.
-->

---

<!-- SLIDE 21: TV3 HUYẾT MẠCH GIAO TIẾP SYNC VS ASYNC -->
# 21. Huyết Mạch Giao Tiếp: Đồng Bộ & Bất Đồng Bộ
### Phần trình bày Thành viên 3 (Mục 2.4, 2.5 & 2.6 — Cơ chế kết nối hệ thống)

<div class="highlight">
<b>Bài toán hóc búa:</b> Sau khi Thành viên 2 đã cô lập hoàn toàn CSDL và cấm tiệt lệnh SQL JOIN chéo, <b>làm thế nào để các module trao đổi dữ liệu với nhau một cách an toàn và tin cậy?</b>
</div>

* **Hai Cơ Chế Giao Tiếp Cốt Lõi:**
  * **1. Giao tiếp Đồng bộ (Synchronous):** Cơ chế "Gọi và Chờ" (Request - Response). Caller gửi yêu cầu và tạm dừng luồng logic cho đến khi nhận được phản hồi.
  * **2. Giao tiếp Bất đồng bộ (Asynchronous):** Cơ chế "Bắn và Quên" (Fire - and - Forget) qua Message Queue. Caller phát sự kiện ra Broker rồi giải phóng luồng ngay lập tức.

<div class="warning-box">
⚠️ <b>Ràng buộc thời gian (Temporal Coupling):</b> Trong gọi đồng bộ, cả bên gọi và bên nhận <b>bắt buộc phải đồng thời trực tuyến</b>. Độ sẵn sàng tổng thể: $A_{\text{total}} = A_1 \times A_2 \times \dots \times A_n$ (Càng gọi nhiều service, hệ thống càng dễ sập!).
</div>

<!-- Speaker Notes:
Kính thưa Thầy và các bạn, em là Thành viên 3. Sau khi bạn Thành viên 2 đã đóng tất cả các cánh cửa truy cập CSDL chung và cấm tiệt SQL JOIN, câu hỏi đặt ra là: Làm sao các module nói chuyện được với nhau?
Đó chính là nhiệm vụ của Huyết mạch Giao tiếp.
Chúng ta có 2 phương thức: Đồng bộ (gọi và chờ) và Bất đồng bộ (phát sự kiện qua Message Queue).
Giao tiếp đồng bộ áp đặt một ràng buộc thời gian nghẹt thở: Cả hai service phải cùng sống tại cùng một thời điểm. Nếu trong chuỗi có một service bị rớt mạng, cả giao dịch sẽ sụp đổ.
-->

---

<!-- SLIDE 22: RESTFUL API VS GRPC -->
# 22. Đối Đầu Công Nghệ Đồng Bộ: RESTful API vs gRPC
### Lựa chọn vũ khí tối ưu cho từng vị trí trong mạng kiến trúc

| Tiêu chí So sánh | RESTful API (HTTP/1.1 + JSON) | gRPC (HTTP/2 + Protobuf) |
|---|---|---|
| **Giao thức mạng** | HTTP/1.1 (phổ biến) hoặc HTTP/2 | HTTP/2 (Chuẩn bắt buộc) |
| **Định dạng dữ liệu** | Văn bản thuần (Text-based JSON) | Nhị phân nén nhỏ gọn (Binary Protobuf) |
| **Khả năng đọc/Debug** | Rất cao; đọc bằng mắt, test Postman dễ dàng | Cần tệp `.proto` để giải mã dữ liệu |
| **Kỷ luật hợp đồng** | Tùy chọn qua Swagger; kiểm tra runtime | Bắt buộc qua `.proto`; Strongly-typed compile-time |
| **Hiệu năng & Trễ** | Trung bình (nghẽn parse text và header lớn) | **Cực cao** (Gấp 5–10 lần, độ trễ < 5ms) |
| **Hỗ trợ Streaming** | Hạn chế (chủ yếu là Unary Request/Response) | Hỗ trợ 4 kiểu: Unary, Client/Server/Bi-di Stream |
| **Vị trí tối ưu** | **Public API Gateway**, Client tới Backend | **Internal Service-to-Service** trong backend |

<div class="highlight" style="margin-top: 8px;">
🎯 <b>Quy tắc vàng:</b> Sử dụng <b>RESTful API</b> làm cửa ngõ đối ngoại cho Frontend/Mobile; và dùng <b>gRPC</b> cho mọi giao tiếp nội bộ giữa các service backend!
</div>

<!-- Speaker Notes:
Khi cần gọi đồng bộ, chúng ta có hai đối thủ lớn: RESTful API và gRPC.
RESTful dùng JSON dạng text, rất dễ đọc và dễ test bằng Postman, sinh ra để làm API đối ngoại cho Web và Mobile.
Nhưng đối với giao tiếp nội bộ giữa các backend service với nhau, gRPC vượt trội hoàn toàn. Nhờ chạy trên nền HTTP/2 ghép kênh (Multiplexing) và mã hóa nhị phân Protobuf, kích thước gói tin giảm 70% và tốc độ nhanh hơn từ 5 đến 10 lần.
Đó là lý do vì sao Order gọi kiểm tra kho sang Inventory ta chọn ngay gRPC!
-->

---

<!-- SLIDE 23: CASCADING FAILURE & CẠN KIỆT THREAD POOL -->
# 23. Góc Tối Của Đồng Bộ: Cascading Failure & Cạn Kiệt Luồng
### Hiểm họa sụp đổ dây chuyền kéo sập toàn bộ máy chủ ứng dụng

* **Cơ chế Lỗi dây chuyền (Cascading Failure):**
  * `Order` gọi đồng bộ `Payment`, `Payment` gọi tiếp `Bank Gateway`.
  * `Bank Gateway` bị chập chờn mạng và treo $30\text{s}$ $\rightarrow$ Hàng loạt thread tại `Payment` và `Order` bị chặn đứng (Blocked)!

```
[Khách hàng] ---> [Order Service] --------> [Payment Service] --------> [Bank Gateway]
                 (Thread: 200/200)         (Thread: 200/200)           (Mạng chập chờn,
                 [CẠN KIỆT THREAD]         [CẠN KIỆT THREAD]            Treo 30 giây!)
                         |
                         v
   HẬU QUẢ: Khách vào xem Catalog / Cart cũng bị LỖI HTTP 503 SERVICE UNAVAILABLE!
```

* **Bộ 4 Giải Pháp Phòng Vệ Bắt Buộc (Resilience Patterns):**
  1. **Timeout & Deadline:** Không bao giờ chờ vô hạn (tối đa 2s giải phóng thread ngay).
  2. **Circuit Breaker (Ngắt mạch tự động):** Lỗi > 50% ngắt mạch ngay, từ chối nhanh (Fail-Fast).
  3. **Bulkhead (Vách ngăn khoang tàu):** Cấp Thread Pool riêng biệt cho từng service.
  4. **Chuyển sang Bất đồng bộ:** Đưa các tác vụ không cần kết quả tức thì sang Message Queue!

<!-- Speaker Notes:
Giao tiếp đồng bộ ẩn chứa một hiểm họa chết người mang tên: Cascading Failure và Thread Pool Exhaustion.
Hãy nhìn vào sơ đồ: Order gọi Payment, Payment gọi cổng ngân hàng. Nếu cổng ngân hàng bị nghẽn mạng và treo 30 giây, các luồng xử lý tại Payment và Order sẽ bị giam giữ. Chỉ sau vài giây, toàn bộ 200 thread trong máy chủ bị cạn kiệt!
Hậu quả thảm khốc: Khách hàng khác vào lướt xem sản phẩm hoặc xem giỏ hàng—những việc không hề liên quan đến thanh toán—cũng bị báo lỗi 503 sập server!
Để sống sót, ta bắt buộc phải cài đặt Timeout, Circuit Breaker, và chuyển bớt tác vụ sang Message Queue.
-->

---

<!-- SLIDE 24: RABBITMQ CỐT LÕI (AMQP 0-9-1) -->
# 24. Giải Phóng Hệ Thống Với Message Queue (RabbitMQ)
### Phi liên kết không gian & thời gian: Điều hòa xung nhịp tải (Load Leveling)

* **Quy tắc bất biến AMQP 0-9-1:** *Producer KHÔNG BAO GIỜ gửi thẳng vào Queue!* Mọi thông điệp đều phải gửi vào Exchange để Exchange phân phối theo Binding.

```
                  +-------------------------------------------------+
                  |               RABBITMQ BROKER                   |
+------------+    |    +------------+        Binding Rules          |
|  PRODUCER  | --Publish-> |  EXCHANGE  | --------------------+     |
+------------+    |    +------------+                     |     |
 (Order Svc)      |          |                            v     |
                  |          | (Route via Key)     +------------+    Consume     +------------+
                  |          +-------------------> |   QUEUE    | -------------> |  CONSUMER  |
                  |                                +------------+  (Manual ACK)  +------------+
                  +-------------------------------------------------+             (Worker)
```

* **Sáu (6) Khái Niệm Cốt Lõi AMQP 0-9-1:**
  1. **Producer:** Bên tạo và phát thông điệp lên Exchange.
  2. **Exchange:** Trạm phân phối thông điệp vào các Queue dựa theo Binding.
  3. **Queue:** Bộ đệm lưu trữ message FIFO an toàn trên RAM/Đĩa cứng.
  4. **Binding:** Cầu nối cấu hình định tuyến giữa Exchange và Queue.
  5. **Routing Key:** Khóa định danh đi kèm message để Exchange so khớp.
  6. **Consumer:** Worker đăng ký lắng nghe Queue, xử lý và gửi tín hiệu ACK xác nhận.

<!-- Speaker Notes:
Để giải phóng luồng xử lý, chúng ta sử dụng RabbitMQ theo chuẩn AMQP 0-9-1.
Message Queue mang lại 3 giá trị vàng: Phi liên kết thời gian (bên gửi và nhận không cần chạy cùng lúc); Phi liên kết không gian (bên gửi không cần biết bên nhận ở đâu); và Khả năng điều hòa tải (Load Leveling) giúp hệ thống không bị quá tải vào giờ cao điểm.
Trong RabbitMQ, hãy nhớ nằm lòng: Producer không bao giờ gửi thẳng vào Queue! Producer chỉ gửi vào Exchange. Exchange sẽ như một bưu điện trung tâm, nhìn vào Routing Key và các Binding để phân phối thư vào đúng các Queue cho Consumer.
-->

---

<!-- SLIDE 25: PHÂN LOẠI 3 LOẠI EXCHANGE -->
# 25. Phân Loại Ba Loại Exchange Trong RabbitMQ
### Định tuyến linh hoạt từ Khớp chính xác, Khớp mẫu đến Phát tán toàn bộ

* **1. Direct Exchange (Khớp chính xác chuỗi ký tự):**
  * So khớp chính xác 100% giữa Routing Key và Binding Key ($O(1)$ Hash Table).
  * *Tình huống sử dụng:* Định tuyến tác vụ định danh cụ thể (ví dụ: `payment.momo`, `payment.vnpay`).

* **2. Topic Exchange (Khớp mẫu đa chiều phân cấp):**
  * Sử dụng các ký tự đại diện để so khớp mẫu phân cấp qua dấu chấm (`.`):
    * `*` (dấu sao): Đại diện cho **chính xác một từ**.
    * `#` (dấu thăng): Đại diện cho **không hoặc nhiều từ**.
  * *Tình huống sử dụng:* Hệ thống ghi log và cảnh báo địa lý (ví dụ: `order.eu.*`, `logs.warning.#`).

* **3. Fanout Exchange (Phát tán toàn bộ - Publish/Subscribe):**
  * **BỎ QUA HOÀN TOÀN ROUTING KEY!**
  * Bất kỳ thông điệp nào tới Fanout Exchange đều được sao chép và phát tán đồng thời tới **TẤT CẢ** các Queue đang liên kết (bind) với nó.
  * Là hiện thân chuẩn mực của mẫu thiết kế **Publish / Subscribe (Pub/Sub)** trong E-Commerce!

<!-- Speaker Notes:
RabbitMQ cung cấp 3 loại Exchange phổ biến:
Thứ nhất là Direct Exchange: So khớp chính xác từng ký tự giữa Routing Key và Binding Key. Tốc độ cực nhanh O(1).
Thứ hai là Topic Exchange: Cho phép so khớp mẫu bằng ký tự wildcard: dấu sao đại diện cho 1 từ, dấu thăng đại diện cho nhiều từ. Rất hay dùng cho hệ thống log phân tán.
Thứ ba là Fanout Exchange: Loại bỏ hoàn toàn Routing Key! Hễ có tin nhắn đến là nó copy nhân bản gửi cho TẤT CẢ các queue đang nối vào nó. Đây chính là exchange hoàn hảo nhất cho bài toán OrderCreated trong TMĐT!
-->

---

<!-- SLIDE 26: SỨC MẠNH FANOUT PUB/SUB -->
# 26. Sức Mạnh Mẫu Thiết Kế Fanout Publish/Subscribe
### Tách biệt hoàn toàn bên phát và các bên nhận theo nguyên tắc Open/Closed

```
                     MÔ HÌNH PUB/SUB QUA FANOUT EXCHANGE
                          +-----------------------+
                          |    Fanout Exchange    |
                          | (ecommerce.order-evt) |
                          +-----------------------+
                                      |
                +---------------------+---------------------+
                |                     |                     |
                v                     v                     v
          +-----------+         +-----------+         +-----------+
          | Queue Inv |         | Queue Not |         | Queue Pro |
          +-----------+         +-----------+         +-----------+
                |                     |                     |
                v                     v                     v
          [Inventory]           [Notification]        [Promotion]
          Trừ kho vật lý        Gửi Mail & SMS        Tích điểm thưởng
```

<div class="highlight">
🌟 <b>Nguyên tắc Open/Closed tuyệt đối:</b> Ngày mai công ty muốn bổ sung Module AI Analytics để phân tích hành vi mua? <b>Chỉ cần tạo thêm 1 Queue mới bind vào Fanout Exchange.</b> Toàn bộ mã nguồn cũ của Order Service và các service khác không cần sửa hay deploy lại dù chỉ một dòng!
</div>

<!-- Speaker Notes:
Sơ đồ này minh họa rõ nét cơ chế Publish/Subscribe qua Fanout Exchange:
Khi Order tạo đơn thành công, nó phát sự kiện OrderCreated lên Exchange. Exchange sẽ tự động nhân bản thông điệp thành 3 bản và đẩy vào 3 Queue độc lập: Queue của Inventory, Queue của Notification, và Queue của Promotion.
Ba Consumer này chạy song song, độc lập hoàn toàn với nhau.
Điều tuyệt vời nhất là tính Open/Closed: Tháng sau sếp bảo muốn thêm tính năng phân tích dữ liệu AI, bạn chỉ việc tạo Queue mới nối vào Exchange. Hệ thống mở rộng thêm tính năng mới mà không phải sửa hay khởi động lại bất kỳ dòng code cũ nào!
-->

---

<!-- SLIDE 27: ĐỘ TIN CẬY RABBITMQ (4 TRỤ CỘT) -->
# 27. Độ Tin Cậy Trong RabbitMQ: 4 Trụ Cột An Toàn Tuyệt Đối
### Bác bỏ định kiến sai lầm: "Bất đồng bộ là dễ mất tin" $\rightarrow$ An toàn vượt trội!

* **Bốn (4) Trụ Cột Đảm Bảo Dữ Liệu Toàn Vẹn 100% (Zero Message Loss):**
  1. **Queue Durability (`durable = true`):** Metadata của hàng đợi được lưu trên đĩa cứng, sống sót qua mọi sự cố khởi động lại Broker.
  2. **Message Persistence (`delivery_mode = 2`):** Thông điệp được ghi vào Disk Write-Ahead Log của máy chủ RabbitMQ trước khi xác nhận.
  3. **Publisher Confirms & Outbox Pattern:** Phía gửi bắt buộc nhận tín hiệu xác nhận ghi đĩa từ Broker; lưu sự kiện vào Outbox Table cùng transaction nội bộ.
  4. **Manual Consumer ACK (`autoAck = false`):** Worker xử lý nghiệp vụ và commit CSDL thành công 100% mới gửi lệnh `basicAck` để xóa tin.

```
[Producer] --Publish(delivery_mode=2)--> [RabbitMQ (Disk Log)] --Deliver--> [Consumer]
                                                    ^                          |
                                                    | (Nếu Crash trước ACK)    v (Commit DB OK)
                                                    +------ Requeue <------- basicAck
```

<!-- Speaker Notes:
Nhiều người có định kiến sai lầm rằng: Dùng Message Queue thì dữ liệu dễ bị mất mát hơn gọi HTTP.
Sự thật hoàn toàn ngược lại! Trong gọi đồng bộ, nếu mạng bị đứt 1 giây, request sụp đổ ngay. Nhưng với RabbitMQ, dữ liệu được bảo vệ kiên cố bằng 4 trụ cột:
Thứ nhất: Queue Durability giữ hàng đợi sống sót qua restart.
Thứ hai: Message Persistence ghi thông điệp thẳng xuống đĩa cứng vật lý chứ không chỉ nằm trên RAM.
Thứ ba: Publisher Confirms và Outbox Pattern đảm bảo tin nhắn không bao giờ bị rơi trên đường bay từ Producer đến Broker.
Thứ tư: Manual ACK—chỉ khi nào consumer xử lý xong và ghi database thành công, nó mới gửi basicAck để xóa tin. Nếu worker bị mất điện giữa chừng, RabbitMQ sẽ tự động hoàn trả tin về hàng đợi để máy khác xử lý lại!
-->

---

<!-- SLIDE 28: ĐỘ TIN CẬY RABBITMQ (THỰC NGHIỆM OFFLINE) -->
# 28. Độ Tin Cậy RabbitMQ: Thực Nghiệm Kịch Bản Consumer Offline
### Chứng minh trực quan: Tắt hoàn toàn Consumer thì thông điệp vẫn bảo toàn 100%

* **Quy Trình 3 Bước Thực Nghiệm Trong Demo Buổi 12:**
  * **Bước 1 (Giả lập sự cố):** Tắt hoàn toàn ứng dụng `Notification Service` $\rightarrow$ Trạng thái Consumer chuyển sang **OFFLINE**.
  * **Bước 2 (Khách đặt hàng):** Người dùng bấm mua hàng $\rightarrow$ `Order Service` chốt đơn và phát `OrderCreatedEvent` lên RabbitMQ $\rightarrow$ Khách nhận phản hồi thành công tức thì (< 50ms) $\rightarrow$ **RabbitMQ lưu an toàn trong Queue trên đĩa cứng (Message Ready = 1)!**
  * **Bước 3 (Hồi phục):** Khởi động lại `Notification Service` $\rightarrow$ Kết nối TCP được tái lập $\rightarrow$ RabbitMQ tự động đẩy ngay message tồn đọng $\rightarrow$ Email/SMS được gửi thành công, gửi ACK xóa tin $\rightarrow$ **Message Ready = 0!**

<div class="highlight">
✔ <b>Kết luận thực chứng:</b> Dữ liệu được bảo toàn tuyệt đối 100%! Bất đồng bộ mang lại độ chịu lỗi (Fault Tolerance) vượt bậc so với gọi đồng bộ.
</div>

<!-- Speaker Notes:
Để chứng minh điều đó, nhóm đã thực hiện kịch bản demo Consumer Offline:
Bước 1: Chúng tôi chủ động tắt hoàn toàn Notification Service.
Bước 2: Chúng tôi bấm đặt hàng. Đơn hàng vẫn tạo thành công, khách nhận thông báo mua xong ngay lập tức. Trên giao diện quản trị RabbitMQ, chúng ta thấy rõ: Queue notification đang chứa đúng 1 message ở trạng thái Ready nằm an toàn trên đĩa cứng!
Bước 3: Chúng tôi bật lại Notification Service. Ngay lập tức, worker kéo tin về, gửi email cho khách và gửi ACK xác nhận xóa tin.
Điều này chứng minh: Dù service nhận có bị sập cả tiếng đồng hồ, không có bất kỳ thông tin nào bị biến mất!
-->

---

<!-- SLIDE 29: LUỒNG THỰC CHIẾN ORDERCREATED -->
# 29. Thực Chiến E-Commerce: Luồng OrderCreated & Bộ Quy Tắc
### Bản giao hưởng kết hợp hoàn hảo giữa Đồng bộ và Bất đồng bộ

```
[Khách hàng]
     | 1. POST /orders (Sync - REST)
     v
[Order Service] ------- 2. CheckStock (Sync - gRPC, < 5ms) -------> [Inventory Service]
     |                                                                    | (Còn hàng)
     | 3. Local Transaction: Ghi đơn (Status = "Confirmed")               |
     | 4. Publish Event "OrderCreated" (Async, delivery_mode=2)           |
     |                                                                    v
     | 5. Trả lời ngay cho Khách hàng: HTTP 201 Created (< 50ms!)
     v
[RabbitMQ Fanout] ===> [Queue Inv] + [Queue Notif] + [Queue Promo] (Xử lý song song ngầm)
```

* **Bộ 4 Quy Tắc Vàng Lựa Chọn Cơ Chế Giao Tiếp:**
  1. **Cần kết quả ngay để quyết định bước tiếp** (Check kho, Xác thực) $\rightarrow$ **ĐỒNG BỘ (gRPC/REST)**.
  2. **Chỉ cần báo một việc đã hoàn tất** (Đã tạo đơn, Gửi email, Tích điểm) $\rightarrow$ **BẤT ĐỒNG BỘ (RabbitMQ)**.
  3. **Yêu cầu nhất quán dữ liệu:** Strong Consistency $\rightarrow$ Sync; Eventual Consistency $\rightarrow$ Async.
  4. **Tác vụ lâu (> 500ms) hoặc phụ thuộc bên thứ 3** (Email/SMS) $\rightarrow$ **BẮT BUỘC ĐƯA VÀO QUEUE**.

<!-- Speaker Notes:
Đây là slide tổng kết kỹ thuật thực chiến: Bản giao hưởng giữa Đồng bộ và Bất đồng bộ.
Khách hàng gửi request tạo đơn.
Bước 1: Order gọi đồng bộ qua gRPC sang Inventory để kiểm tra kho. Bước này bắt buộc phải đồng bộ vì phải biết chắc còn hàng mới cho tạo đơn. Nhờ gRPC, bước này chỉ mất dưới 5ms.
Bước 2: Order ghi đơn vào database nội bộ.
Bước 3: Order phát sự kiện OrderCreated lên RabbitMQ rồi trả về HTTP 201 Created cho khách hàng ngay lập tức! Toàn bộ luồng chỉ mất dưới 50ms.
Sau đó, việc trừ kho ngầm, gửi email, tích điểm thẻ thành viên diễn ra bất đồng bộ phía sau mà không bắt khách hàng phải đứng chờ xoay vòng xoay màn hình!
-->

---

<!-- SLIDE 30: TỔNG KẾT NHÓM: KIỀNG 3 CHÂN -->
# 30. Tổng Kết Toàn Nhóm: Mối Quan Hệ Biện Chứng "Kiềng 3 Chân"
### Sự gắn kết hữu cơ không thể tách rời giữa 3 mảng nghiên cứu

```
                    TRỤ CỘT 1: KIẾN TRÚC TỔNG THỂ (TV1)
                     * Chiến lược: MonolithFirst & Hybrid
                     * Nền tảng: Modular Monolithic
                     * Lộ trình tiến hóa: 4 bước chuẩn mực
                                    |
                         (Định hình tư duy phân rã)
                                    v
                    TRỤ CỘT 2: RANH GIỚI NGHIỆP VỤ (TV2)
                     * Phương pháp luận: Bounded Context & DDD
                     * Công cụ: Bộ 4 câu hỏi vàng phân định
                     * Kỷ luật: CÔ LẬP SCHEMA DỮ LIỆU "THÉP"
                                    |
                         (Cấm JOIN chéo DB -> Đòi hỏi giao tiếp mới)
                                    v
                    TRỤ CỘT 3: CƠ CHẾ GIAO TIẾP (TV3)
                     * Đồng bộ: gRPC siêu tốc, xóa Cascading Failure
                     * Bất đồng bộ: RabbitMQ Fanout Pub/Sub tin cậy 100%
                     * Hiện thực hóa lộ trình 4 bước của TV1
```

<!-- Speaker Notes:
Kính thưa Thầy và các bạn, toàn bộ bài báo cáo Buổi 12 gắn kết chặt chẽ theo mô hình "Kiềng 3 chân" hữu cơ:
- Trụ cột 1 của TV1 định hình tầm nhìn vĩ mô: Modular Monolith là bước đệm tối ưu. Nhưng để xây được nó, ta cần phương pháp luận.
- Trụ cột 2 của TV2 mang đến phương pháp DDD và Bộ 4 câu hỏi vàng để bóc tách 10 module, đồng thời thiết lập kỷ luật cô lập dữ liệu thép: Cấm tuyệt đối SQL JOIN chéo bảng.
- Khi cánh cửa database chung bị đóng lại, Trụ cột 3 của TV3 chính là chiếc chìa khóa mở ra sự kết nối: dùng gRPC để gọi đồng bộ kiểm tra kho, và dùng RabbitMQ để phát tán sự kiện bất đồng bộ.
3 trụ cột tương hỗ lẫn nhau, tạo nên một hệ thống vững chắc như bàn thạch!
-->

---

<!-- SLIDE 31: BÀI HỌC CỐT LÕI CHO KỸ SƯ PHẦN MỀM -->
# 31. Ba Bài Học Cốt Lõi Cho Kỹ Sư Phần Mềm Hiện Đại
### Đúc kết giá trị thực chiến từ toàn bộ chuyên đề Buổi 12

<div class="highlight">
<b>Bài học 1: "Không có kiến trúc tốt nhất, chỉ có kiến trúc phù hợp nhất"</b>
<br>• Microservices không phải là đích đến bắt buộc của mọi dự án.
<br>• Modular Monolithic chính là "Sweet Spot" hoàn hảo nhất cho đa số bài toán doanh nghiệp hiện nay.
</div>

<div class="highlight">
<b>Bài học 2: "Kỷ luật ranh giới quan trọng hơn công nghệ triển khai"</b>
<br>• Dù chạy 1 process hay 100 services: <b>CẤM TUYỆT ĐỐI SQL JOIN chéo dữ liệu</b>!
<br>• Kỷ luật ranh giới lỏng lẻo sẽ biến hệ thống thành "Big Ball of Mud" hoặc "Distributed Monolith".
</div>

<div class="highlight">
<b>Bài học 3: "Phối hợp thông minh giữa Đồng bộ và Bất đồng bộ"</b>
<br>• Dùng <b>Đồng bộ (gRPC)</b> cho những quyết định chốt chặn sống còn tức thì.
<br>• Trao quyền cho <b>Message Queue (RabbitMQ)</b> xử lý bất đồng bộ ở tất cả các khâu còn lại để tối ưu hóa thông lượng và bảo toàn dữ liệu tuyệt đối.
</div>

<!-- Speaker Notes:
Cuối cùng, nhóm xin gửi tới các bạn 3 bài học đắt giá nhất:
Thứ nhất: Đừng bao giờ chạy theo trào lưu Microservices một cách mù quáng. Modular Monolithic mang lại sự cân bằng hoàn hảo giữa tính đơn giản trong vận hành và sự ngăn nắp trong mã nguồn cho đại đa số hệ thống.
Thứ hai: Hãy giữ vững kỷ luật ranh giới dữ liệu. Một khi các bạn đã để cho các câu lệnh SQL JOIN chéo bảng lọt vào codebase, các bạn sẽ vĩnh viễn không bao giờ có thể tiến hóa hệ thống lên mây hay microservices được nữa.
Thứ ba: Hãy kết hợp nhuần nhuyễn giữa gRPC và RabbitMQ. Đồng bộ cho hiện tại, và Bất đồng bộ cho tương lai!
Xin chân thành cảm ơn Thầy và các bạn đã chú ý lắng nghe bài thuyết trình của nhóm!
-->
