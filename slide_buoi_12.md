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
    font-size: 21px;
    padding-top: 50px;
    padding-bottom: 26px;
    padding-left: 42px;
    padding-right: 42px;
    color: #1e293b;
    background-color: #ffffff;
    line-height: 1.42;
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
  .tag-theory {
    background-color: #eff6ff;
    color: #1d4ed8;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 14px;
    font-weight: 700;
    vertical-align: middle;
  }
  .tag-demo {
    background-color: #f0fdf4;
    color: #15803d;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 14px;
    font-weight: 700;
    vertical-align: middle;
  }
  .tag-ext {
    background-color: #fefce8;
    color: #a16207;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 14px;
    font-weight: 700;
    vertical-align: middle;
  }
  .highlight {
    background-color: #f8fafc;
    padding: 9px 13px;
    border: 1px solid #e2e8f0;
    border-left: 4px solid #2563eb;
    border-radius: 4px;
    margin: 8px 0;
    font-size: 18.5px;
    color: #1e293b;
  }
  .warning-box {
    background-color: #fffdf5;
    padding: 9px 13px;
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
    font-size: 14.5px;
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
    padding: 8px 10px;
    font-size: 13.5px;
    line-height: 1.25;
    color: #0f172a;
  }
  code {
    font-family: 'Consolas', 'Fira Code', 'Courier New', monospace;
    color: #0f172a;
  }
---

<!-- SLIDE 0: TITLE SLIDE -->
# MODULAR MONOLITHIC <span class="tag-theory">[LÝ THUYẾT]</span>
## Tư duy Thiết kế & Con đường Tiến hóa lên Microservice

**Học phần:** Phát triển Hệ thống Server Nâng cao — Buổi 12  
**Đơn vị:** Bộ môn Kỹ thuật Phần mềm — Khoa Công nghệ Thông tin

<div class="highlight" style="margin-top: 12px;">
<b>Phân công Nghiên cứu & Báo cáo:</b>
<br>• <b>Thành viên 1 (TV1):</b> Tổng quan Kiến trúc, Tiêu chí Lựa chọn & Lộ trình Tiến hóa
<br>• <b>Thành viên 2 (TV2):</b> Tư duy Phân rã Nghiệp vụ (DDD), Bounded Context & 10 Module
<br>• <b>Thành viên 3 (TV3):</b> Huyết mạch Giao tiếp Đồng bộ (Sync) & Bất đồng bộ (RabbitMQ)
</div>

* **Câu chuyện xuyên suốt bài học:** *"Khách hàng đặt mua chiếc Laptop Gaming cuối cùng trong đợt Flash Sale".*

<!-- Speaker Notes:
Kính chào Thầy và các bạn. Hôm nay nhóm xin báo cáo chuyên đề Buổi 12: "Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservice".
Chuyên đề được dẫn dắt qua câu chuyện thực tế: "Khách hàng đặt mua chiếc Laptop Gaming cuối cùng", theo thế Kiềng 3 chân:
- TV1: Định vị mô hình Monolith vs Microservices và tiêu chí lựa chọn thực tế.
- TV2: Phương pháp luận Domain-Driven Design phân rã 10 module E-commerce và kỷ luật cô lập dữ liệu.
- TV3: Cơ chế kết nối hệ thống bằng Đồng bộ kiểm tra kho và Bất đồng bộ qua RabbitMQ.
-->

---

<!-- SLIDE 1: MONOLITH VS MICROSERVICES -->
# 1. Đặt Vấn Đề: Cuộc Đối Đầu Giữa Hai Thái Cực <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 1 — Phân tích sự đánh đổi (Trade-offs) trong thiết kế hệ thống

<div class="grid-2">
<div>

### 🏢 Monolithic Truyền Thống
* **Bản chất:** 1 Codebase, 1 Process, 1 CSDL chung.
* **Ưu điểm:** Đơn giản, nhấn F5 chạy ngay, giao dịch ACID trực tiếp trong 1 database.
* **Hiểm họa khi tăng trưởng:** Mã nguồn hóa thành **Spaghetti Code** (mớ bòng bong); 1 lỗi nhỏ kéo sập cả hệ thống (Single Point of Failure).

</div>
<div>

### 🌐 Microservices Phân Tán
* **Bản chất:** Đa Repo/Process, Database-per-Service.
* **Lợi ích:** Scale co giãn độc lập từng service tải cao; các team deploy song song.
* **Khoản thuế phân tán:** Chi phí hạ tầng và vận hành DevOps cực lớn; mất ACID phải dùng Saga.

</div>
</div>

<div class="highlight" style="margin-top: 8px;">
🏠 <b>Ví von đời thường:</b> Monolith như một <b>căn nhà tập thể ở chung</b> (tiện sinh hoạt nhưng 1 phòng cháy thì cả nhà nguy khốn); Microservices như <b>mỗi người mua hẳn một ngôi nhà riêng ngoài phố</b> (tự do nhưng chi phí đắt đỏ và việc đi lại gặp nhau rất tốn thời gian).
</div>

<!-- Speaker Notes:
Mở đầu phần TV1, chúng ta nhìn lại 2 thái cực kiến trúc:
Monolith truyền thống như căn nhà tập thể: rất tiện ban đầu nhưng lâu dần code rối như mớ bòng bong.
Microservices chia nhỏ thành nhà riêng ngoài phố: mỗi người tự do nhưng chi phí vận hành hạ tầng DevOps đè nặng lên doanh nghiệp.
-->

---

<!-- SLIDE 2: MODULAR MONOLITHIC SWEET SPOT -->
# 2. Modular Monolithic: Điểm Cân Bằng Tối Ưu <span class="tag-theory">[LÝ THUYẾT]</span>
### Vận hành đơn khối nhưng tổ chức mã nguồn ngăn nắp theo ranh giới nghiệp vụ

<div class="highlight">
<b>Định nghĩa:</b> Vẫn là <b>1 Process duy nhất, 1 lần deploy</b> (đơn giản như Monolith), nhưng code được <b>đóng gói thành các module độc lập</b> theo nghiệp vụ, giao tiếp qua Interface thay vì gọi chéo tùy tiện.
</div>

```
+-------------------------------------------------------------------------+
|                        MODULAR MONOLITH RUNTIME                         |
|                       (1 Tiến trình thực thi duy nhất)                  |
|                                                                         |
|   [ ORDER MODULE ]             In-Memory Call           [ INVENTORY ]   |
|   - Order Logic             ======================>     - StockItem     |
|   - DbContext (internal)      (IInventoryService)       - Db (internal) |
+-------------------------------------------------------------------------+
            |                                                  |
     (order.orders)                                     (inventory.stocks)
            v                                                  v
+-------------------------------------------------------------------------+
|                     SINGLE PHYSICAL DATABASE SERVER                     |
|           (Dùng chung 1 DB Server nhưng CÔ LẬP THEO 3 SCHEMAS)          |
|      * Schema 'order'       * Schema 'inventory'     * Schema 'notif'   |
+-------------------------------------------------------------------------+
```

🏢 **Ví von:** Như **một tòa nhà chung nhưng chia 3 phòng khép kín có khóa riêng**; mỗi phòng giữ chìa khóa tủ hồ sơ của mình, chỉ nói chuyện qua cửa sổ giao dịch!

<!-- Speaker Notes:
Modular Monolith là giải pháp dung hòa đỉnh cao: Vận hành chỉ 1 process duy nhất, nhưng mã nguồn chia thành các Bounded Context riêng biệt.
Tòa nhà chia phòng khép kín: Phòng Đơn hàng, Phòng Kho, Phòng Thông báo. Mỗi phòng giữ schema CSDL riêng, không ai được tự ý đọc bảng của nhau.
-->

---

<!-- SLIDE 3: BỘ 3 TIÊU CHÍ LỰA CHỌN -->
# 3. Bộ 3 Tiêu Chí Lựa Chọn Thực Tế & MonolithFirst <span class="tag-theory">[LÝ THUYẾT]</span>
### Chọn kiến trúc theo nguồn lực và giai đoạn sản phẩm, không chạy theo trào lưu

<div class="grid-3">
<div>

### 👥 1. Quy Mô Đội Ngũ
*(Định luật Conway)*
* **Nhóm nhỏ (1–10 người):** Modular Monolith tối ưu nhất; tập trung làm tính năng.
* **Nhiều Team độc lập (> 30):** Microservices giúp tránh xung đột deploy.

</div>
<div>

### 🛠️ 2. Hạ Tầng & DevOps
*(Chi Phí Vận Hành)*
* **Thiếu DevOps chuyên sâu:** Làm Microservices sẽ kiệt sức vì quản lý container và mạng.
* **Modular Monolith:** 1 pipeline CI/CD, chi phí máy chủ tối thiểu.

</div>
<div>

### ⏱️ 3. Giai Đoạn Sản Phẩm
*(Độ Trưởng Thành)*
* **MVP / Mới bắt đầu:** Nghiệp vụ thay đổi liên tục $ightarrow$ Modular Monolith refactor rất nhanh.
* **Trưởng thành:** Điểm nghẽn tải rõ ràng $ightarrow$ Tách đúng module đó.

</div>
</div>

<div class="highlight" style="margin-top: 10px;">
🏆 <b>Nguyên lý "MonolithFirst" (Martin Fowler - 2015):</b> Luôn bắt đầu bằng Modular Monolithic, chỉ tách module thành microservice khi có lý do thực tế rõ ràng!
<br>• <i>Shopify:</i> Nền tảng TMĐT tỷ đô vận hành Modular Monolith khổng lồ với hơn 2.8 triệu dòng code mà không cần đập bỏ để chia nhỏ.
</div>

<!-- Speaker Notes:
Quy tắc vàng: Kiến trúc phải phù hợp với nguồn lực.
Định luật Conway chỉ ra rằng cấu trúc phần mềm phản ánh cơ cấu tổ chức. Đội nhỏ làm Microservices sẽ đốt sạch năng lượng vào việc sửa hạ tầng.
Nguyên tắc MonolithFirst của Martin Fowler khuyên chúng ta luôn bắt đầu với Modular Monolith, như case study kinh điển của Shopify.
-->

---

<!-- SLIDE 4: KIẾN TRÚC LAI & LỘ TRÌNH 4 BƯỚC TIẾN HÓA -->
# 4. Kiến Trúc Lai (Hybrid) & Lộ Trình 4 Bước Tiến Hóa <span class="tag-theory">[LÝ THUYẾT]</span>
### Bóc tách module Inventory thành Microservice mà không sửa đổi logic nghiệp vụ

* **Mô hình Kiến trúc Lai (Hybrid):** Giữ 80–90% nghiệp vụ trong Monolith lõi; chỉ bóc tách 1–2 module đặc thù (như Inventory chịu tải cao mùa Flash Sale, Payment cần bảo mật).

```
   [ BƯỚC 1: TÁCH DATABASE RIÊNG ]        -> Schema 'inventory' đã độc lập, mang sang DB server riêng
                 |
   [ BƯỚC 2: ĐỔI INTERFACE SANG NETWORK ]  -> Thay Adapter in-process bằng Http Client (Order không đổi logic!)
                 |
   [ BƯỚC 3: ĐÓNG GÓI DOCKER RIÊNG ]       -> Viết Dockerfile Multi-stage build riêng (< 150MB, kế thừa B6, 7)
                 |
   [ BƯỚC 4: PIPELINE CI/CD RIÊNG ]        -> Thiết lập GitHub Actions với path trigger (kế thừa B10, 11)
```

<div class="highlight">
🚪 <b>Ví von:</b> Như việc <b>dọn riêng phòng Kho ra thuê một mặt bằng riêng ngoài phố</b>; các phòng khác trong tòa nhà vẫn làm việc bình thường, chỉ chuyển sang gọi điện thoại liên lạc!
</div>

<!-- Speaker Notes:
Khi cần scale module Kho trong dịp Flash Sale, ta không cần đập bỏ toàn bộ hệ thống. Ta áp dụng lộ trình 4 bước: Tách CSDL, Đổi interface sang network call, Đóng gói Docker riêng và tạo CI/CD riêng.
Logic nghiệp vụ được bảo toàn 100% nhờ Dependency Inversion.
-->

---

<!-- SLIDE 5: TV2 PHÂN RÃ NGHIỆP VỤ DDD -->
# 5. Tư Duy Phân Rã Nghiệp Vụ: Cắt Lát Theo Chiều Dọc <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 2 (Mục 2.2 — Trọng tâm khó nhất): Domain-Driven Design (DDD)

<div class="grid-2">
<div>

### ❌ Chia Ngang Kỹ Thuật (Sai Lầm)
* 1 folder `Controllers`, 1 folder `Services`, 1 folder `Repositories` dùng chung cho toàn bộ app.
* **Hậu quả:** 1 tính năng bị xé vụn ra 3 nơi; sửa 1 logic phải đụng vào nhiều file; các service inject chéo nhau thành mạng nhện.

</div>
<div>

###  Chia Dọc Nghiệp Vụ (DDD Chuẩn)
* Cắt lát hệ thống thành các **Bounded Context** khép kín từ API, Business Logic đến Database Schema.
* Mỗi module bao trùm một bài toán trọn vẹn; bảo vệ ranh giới phòng vệ nội bộ.

</div>
</div>

```
MÔ HÌNH CHIA NGANG (SAI - RỐI MÃ NGUỒN)       MÔ HÌNH CHIA DỌC (ĐÚNG - MODULAR MONOLITH)
[ Controllers chung ]                        +-----------------+ +-----------------+ +-----------------+
[ Services gọi chéo lung tung ]              | CATALOG MODULE  | |  ORDER MODULE   | |INVENTORY MODULE |
[ Repositories / DbContext chung ]           | - Api / Domain  | | - Api / Domain  | | - Api / Domain  |
                                             | - Schema riêng  | | - Schema riêng  | | - Schema riêng  |
                                             +-----------------+ +-----------------+ +-----------------+
```

<!-- Speaker Notes:
Phần trọng tâm của TV2: Làm sao để chia module đúng mà không chia theo cảm tính?
Sai lầm phổ biến là chia ngang kỹ thuật theo thư mục. Ta phải chia dọc theo Bounded Context: mỗi module khép kín từ giao diện đến CSDL, độc lập và tự chủ.
-->

---

<!-- SLIDE 6: BOUNDED CONTEXT & ĐA NGHĨA PRODUCT -->
# 6. Bounded Context & Ngôn Ngữ Chung (Ubiquitous Language) <span class="tag-theory">[LÝ THUYẾT]</span>
### Sự đa nghĩa của thực thể "Product" — Ảo tưởng về một Entity chung toàn năng

<div class="highlight">
<b>Quy tắc vàng DDD:</b> Một thuật ngữ chỉ có ý nghĩa chính xác bên trong một ngữ cảnh nghiệp vụ cụ thể. Gom chung tất cả vào 1 class sẽ tạo ra <b>God Class</b> phình to 70 thuộc tính!
</div>

* **Bốn (4) lăng kính khác nhau của thực thể "Product":**
  * **1. Catalog Context:** Sản phẩm chào bán (Tên đẹp, Ảnh lung linh, Mô tả SEO, Giá niêm yết).
  * **2. Inventory Context:** Đơn vị lưu kho SKU (Mã vạch Barcode, Kệ kho Aisle/Bin, Tồn khả dụng).
  * **3. Shipping Context:** Kiện hàng giao vận (Cân nặng gram, Thể tích Dài x Rộng x Cao, Dễ vỡ).
  * **4. Order Context:** **Bản chụp bất biến (Immutable Snapshot)** (Tên và đơn giá tại thời điểm bấm mua).

<div class="warning-box">
⚠️ <b>Tính bất biến sống còn:</b> Khách mua chiếc Laptop cuối cùng giá 30 triệu hôm nay; ngày mai Catalog tăng giá lên 35 triệu thì đơn hàng cũ <b>tuyệt đối không được phép đổi giá theo</b>!
</div>

<!-- Speaker Notes:
Khái niệm Ubiquitous Language chỉ ra rằng cùng là chữ "Product", nhưng ở Catalog nó là ảnh đẹp chào bán; ở Kho nó là vị trí kệ hàng; và ở Đơn hàng nó là Bản chụp bất biến (Snapshot).
Không bao giờ được SQL JOIN trực tiếp Order sang Catalog!
-->

---

<!-- SLIDE 7: BỘ 4 CÂU HỎI VÀNG PHÂN ĐỊNH MODULE -->
# 7. Bộ 4 Câu Hỏi Vàng Phân Định Ranh Giới Module <span class="tag-theory">[LÝ THUYẾT]</span>
### Công cụ tư duy khách quan, khoa học — Loại bỏ hoàn toàn sự cảm tính cá nhân

| # | Câu hỏi vàng | Bản chất kỹ thuật & Nghiệp vụ | Mục tiêu bảo vệ |
|---|---|---|---|
| **1** | **Ngôn ngữ nghiệp vụ riêng?** | Nghiệp vụ có hệ thống thuật ngữ, khái niệm đặc thù không? | Chống ép khuôn mô hình (Model Smashing) |
| **2** | **Ranh giới tổ chức / phòng ban?** | Phòng ban nào trong thực tế chịu trách nhiệm KPI và sở hữu? | Đồng quy Định luật Conway, xác định Product Owner |
| **3** | **Độc lập dữ liệu (No-JOIN)?** | Dữ liệu có thể lưu riêng mà KHÔNG CẦN viết SQL JOIN chéo? | Triệt tiêu phụ thuộc CSDL, sẵn sàng bóc tách |
| **4** | **Tốc độ thay đổi yêu cầu?** | Tần suất thay đổi nhanh (theo tuần) hay ổn định (theo năm)? | Cô lập phạm vi ảnh hưởng lỗi (Blast Radius) |

<div class="highlight" style="margin-top: 10px;">
🎯 <b>Ứng dụng thực tế:</b> Thỏa mãn cả 4 câu hỏi là điều kiện tiên quyết để định hình một Module độc lập vững chắc.
</div>

<!-- Speaker Notes:
Để không tranh cãi cảm tính, nhóm áp dụng Bộ 4 câu hỏi vàng:
Ngôn ngữ có riêng không? Phòng ban nào sở hữu? Dữ liệu có đứng độc lập không cần SQL JOIN không? Tốc độ thay đổi thế nào?
-->

---

<!-- SLIDE 8: MA TRẬN 10 NGHIỆP VỤ (PHẦN 1) -->
# 8. Ma Trận 10 Nghiệp Vụ E-Commerce (Phần 1: Module 1 – 5) <span class="tag-theory">[LÝ THUYẾT]</span>
### Khảo sát nhóm nghiệp vụ Giao dịch & Vận hành theo Bộ 4 câu hỏi vàng

| STT | Nghiệp vụ | 1. Ngôn ngữ riêng | 2. Phòng ban | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định |
|:---:|---|---|---|---|:---:|:---:|
| **1** | **Catalog** | Specs, SEO, Giá niêm yết | Merchandising | Độc lập cao: Lưu thông tin mô tả, ảnh | Thấp - TB | **TÁCH RIÊNG** |
| **2** | **Cart** | CartItem, Giỏ tạm, Session | Growth & UX | Độc lập cao: Lưu Redis/Schema riêng, xóa sau 7 ngày | Trung bình | **TÁCH RIÊNG** |
| **3** | **Order** | SalesOrder, Hợp đồng, Snapshot | Vận hành Đơn | Độc lập cao: Lưu **Snapshot** sản phẩm; cấm JOIN | Thấp - TB | **TÁCH LÕI** |
| **4** | **Payment** | Giao dịch, Đối soát, Refund | Kế toán - Tài chính| Tuyệt đối: Lưu log đối soát, liên kết qua OrderId | Thấp (PCI-DSS)| **TÁCH RIÊNG** |
| **5** | **Inventory**| SKU, Vị trí kệ, Tồn khả dụng | Quản lý Kho | Độc lập cao: Quản lý số lượng tồn theo SKU | Thấp | **TÁCH RIÊNG** |

<!-- Speaker Notes:
Ma trận 5 module đầu tiên: Catalog và Cart tách biệt; Order là hạt nhân giao dịch lưu snapshot; Payment tuân thủ chuẩn tài chính; Inventory quản lý vị trí kho và tồn khả dụng.
-->

---

<!-- SLIDE 9: MA TRẬN 10 NGHIỆP VỤ (PHẦN 2) -->
# 9. Ma Trận 10 Nghiệp Vụ E-Commerce (Phần 2: Module 6 – 10) <span class="tag-theory">[LÝ THUYẾT]</span>
### Khảo sát nhóm nghiệp vụ Hỗ trợ & Tiếp thị mở rộng

| STT | Nghiệp vụ | 1. Ngôn ngữ riêng | 2. Phòng ban | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định |
|:---:|---|---|---|---|:---:|:---:|
| **6** | **Shipping** | Vận đơn (Waybill), Kiện hàng | Logistics & 3PL | Độc lập cao: Lưu địa chỉ giao, cân nặng, mã tracking | Trung bình | **TÁCH RIÊNG** |
| **7** | **Identity** | UserAccount, Role, Token JWT | An ninh mạng & IT | Tuyệt đối: Lưu tài khoản, mật khẩu; module khác lưu UserId | Rất thấp | **TÁCH RIÊNG** |
| **8** | **Notification**| Template, Kênh (SMS/Email/Push)| CSKH | Tuyệt đối: Nhận payload từ Queue; không lưu data lâu dài | Trung bình | **TÁCH RIÊNG** |
| **9** | **Promotion** | Voucher, Quota, Flash Sale | Marketing | Độc lập cao: Tính tiền giảm dựa trên DTO đầu vào | **RẤT CAO** | **TÁCH RIÊNG** |
| **10**| **Review** | Rating 1-5 sao, Verified Badge | Quản trị Chất lượng| Độc lập cao: Lưu đánh giá theo ProductId và UserId | Thấp | **TÁCH RIÊNG** |

<div class="warning-box" style="margin-top: 8px;">
🔥 <b>Điểm nóng Promotion:</b> Tốc độ thay đổi RẤT CAO theo từng chiến dịch sale. Tách riêng Promotion là lá chắn bảo vệ khối Order lõi không bị ảnh hưởng mỗi khi Marketing đổi luật giảm giá!
</div>

<!-- Speaker Notes:
5 module tiếp theo: Shipping giao vận, Identity xác thực, Notification gửi tin, Review đánh giá, và đặc biệt Promotion có tốc độ thay đổi rất cao nên bắt buộc tách riêng để cô lập vùng ảnh hưởng.
-->

---

<!-- SLIDE 10: CẤU TRÚC CLEAN ARCHITECTURE & 4 KỶ LUẬT CÔ LẬP -->
# 10. Cấu Trúc Module & Bốn (4) Kỷ Luật Cô Lập CSDL "Thép" <span class="tag-theory">[LÝ THUYẾT]</span>
### Kế thừa Clean Architecture Buổi 4: Mỗi module là một ứng dụng độc lập thu nhỏ

<div class="grid-2">
<div>

### 🏗️ Cấu Trúc Nội Bộ Module
* **Domain:** Entity thuần khiết (CẤM dính ORM).
* **Application:** Use Cases điều phối luồng.
* **Infrastructure:** `DbContext` nội bộ (**Khai báo internal**).
* **Contracts:** Interface công khai & DTOs nguyên thủy (`IInventoryService`).

</div>
<div>

### 🛡️ 4 Kỷ Luật Cô Lập "Thép"
1. **Schema riêng biệt:** `order.*`, `inventory.*`, `notif.*`.
2. **CẤM TUYỆT ĐỐI SQL JOIN xuyên Schema.**
3. **CẤM truy cập chéo DbContext / Repository.**
4. **CẤM tạo Foreign Key cứng ở tầng CSDL.**

</div>
</div>

<div class="highlight" style="margin-top: 8px;">
🔒 <b>Tự động hóa kiểm soát:</b> Tích hợp <b>NetArchTest</b> vào CI/CD để chặn đứng mọi code vi phạm ranh giới kiến trúc ngay tại thời điểm build!
</div>

<!-- Speaker Notes:
Kỷ luật cốt lõi của Modular Monolith:
Cấu trúc Clean Architecture thu nhỏ trong từng module; DbContext khai báo internal để không ai inject chéo được; 4 điều luật giới nghiêm cô lập CSDL được kiểm soát tự động bằng NetArchTest.
-->

---

<!-- SLIDE 11: TV3 GIAO TIẾP ĐỒNG BỘ VS BẤT ĐỒNG BỘ -->
# 11. Huyết Mạch Giao Tiếp: Đồng Bộ & Bất Đồng Bộ <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 3 (Mục 2.4, 2.5, 2.6): Cơ chế kết nối các module khi CSDL đã bị cô lập

* **Thách thức:** Cửa truy cập CSDL chung đã bị đóng lại $ightarrow$ Giao tiếp trở thành huyết mạch sống còn!

```
+------------------------------------+------------------------------------+
|   GIAO TIẾP ĐỒNG BỘ (SYNCHRONOUS)  | GIAO TIẾP BẤT ĐỒNG BỘ (ASYNC - MQ) |
+------------------------------------+------------------------------------+
| * Cơ chế: Request - Response       | * Cơ chế: Publish - Subscribe      |
| * Ràng buộc: Cùng trực tuyến       | * Ràng buộc: Phi liên kết thời gian|
| * Bản chất: "Gọi và Chờ kết quả"   | * Bản chất: "Thả thư và Đi"       |
| * Áp dụng: Cần kết quả NGAY LẬP TỨC| * Áp dụng: Chạy ngầm, không bắt chờ|
+------------------------------------+------------------------------------+
```

<div class="highlight">
📞 <b>Ví von đời thường:</b> 
<br>• <b>Đồng bộ (Sync):</b> Như <b>gọi điện thoại</b> hỏi kho "Còn chiếc laptop cuối cùng không?" và giữ máy chờ trả lời ngay.
<br>• <b>Bất đồng bộ (Async):</b> Như <b>thả phiếu vào hòm thư</b> báo tin "Đã bán xong chiếc laptop", các phòng ban tự mở hòm thư lấy việc ra làm!
</div>

<!-- Speaker Notes:
Phần của TV3: Khi dữ liệu đã bị cô lập, ta kết nối bằng Đồng bộ và Bất đồng bộ.
Ví von: Đồng bộ như gọi điện thoại trực tiếp chờ trả lời ngay. Bất đồng bộ như thả phiếu vào hòm thư RabbitMQ để các phòng tự lấy ra xử lý.
-->

---

<!-- SLIDE 12: CÔNG NGHỆ ĐỒNG BỘ: REST VS GRPC -->
# 12. Công Nghệ Đồng Bộ: RESTful API vs gRPC <span class="tag-theory">[LÝ THUYẾT]</span>
### Minh bạch công nghệ: Demo dùng REST loopback; gRPC là giải pháp khi tách Service

| Tiêu chí | RESTful API (HTTP/1.1 + JSON) | gRPC (HTTP/2 + Protocol Buffers) |
|---|---|---|
| **Định dạng dữ liệu** | Văn bản JSON dễ đọc, dễ test Postman | Nhị phân nén nhỏ gọn, hợp đồng `.proto` |
| **Giao thức mạng** | HTTP/1.1 tuần tự | HTTP/2 Multiplexing (Dồn kênh trên 1 TCP) |
| **Vị trí tối ưu** | **Public API Gateway**, Client tới Server | **Internal Inter-Service** giữa các backend pods |
| **Áp dụng trong bài** | **ĐÃ DÙNG THẬT TRONG DEMO** (HttpClient Loopback)| **Lựa chọn tối ưu khi tách thành Microservice** |

```
HTTP/1.1 (REST): [TCP Handshake] -> [Request 1] -> [Response 1] -> [Request 2] (Tuần tự)
HTTP/2 (gRPC):   [Duy nhất 1 kết nối TCP] => [Frame 1][Frame 2][Frame 1] (Song song siêu tốc)
```

<div class="highlight">
🎯 <b>Đúng bản chất:</b> Trong phạm vi Modular Monolith, demo sử dụng REST HttpClient; gRPC là khuyến nghị kỹ thuật cao cấp khi bóc tách hoàn toàn sang microservice độc lập.
</div>

<!-- Speaker Notes:
So sánh REST và gRPC: REST dùng JSON dễ đọc, gRPC dùng HTTP/2 Protobuf nhị phân siêu tốc.
Lưu ý trung thực: Trong bài demo hôm nay nhóm dùng REST loopback qua HttpClient; gRPC là lựa chọn khi tách service riêng biệt.
-->

---

<!-- SLIDE 13: CASCADING FAILURE & RABBITMQ BROKER -->
# 13. Rủi Ro Cascading Failure & Giải Phóng Bằng RabbitMQ <span class="tag-theory">[LÝ THUYẾT]</span>
### Tránh cạn kiệt luồng xử lý bằng kiến trúc hướng sự kiện (AMQP 0-9-1)

* **Hiểm họa sụp đổ dây chuyền (Cascading Failure):** Nếu Order gọi đồng bộ sang cổng thanh toán hoặc email mà bên nhận bị treo $ightarrow$ Toàn bộ Thread Pool (200/200 threads) bị nghẽn $ightarrow$ Khách vào xem web cũng bị lỗi 503 sập server!
* **Quy tắc bất biến RabbitMQ:** *Producer KHÔNG BAO GIỜ gửi thẳng vào Queue!* Mọi thông điệp đều phải gửi vào Exchange để Exchange phân phối theo Binding.

```
                  +-------------------------------------------------+
                  |               RABBITMQ BROKER                   |
+------------+    |    +------------+        Binding Rules          |
|  PRODUCER  | --Publish-> |  EXCHANGE  | --------------------+     |
+------------+    |    +------------+                     |     |
 (Order Svc)      |          |                            v     |
                  |          |                     +------------+    Consume     +------------+
                  |          +-------------------> |   QUEUE    | -------------> |  CONSUMER  |
                  |                                +------------+  (Manual ACK)  +------------+
                  +-------------------------------------------------+
```

<!-- Speaker Notes:
Gọi đồng bộ nhiều tầng tiềm ẩn rủi ro Cascading Failure: 1 service treo làm cạn kiệt toàn bộ Thread Pool của server.
Giải pháp là đưa các tác vụ không cần kết quả tức thì vào RabbitMQ. Producer gửi vào Exchange, Exchange phân phối vào các Queue độc lập.
-->

---

<!-- SLIDE 14: FANOUT PUB/SUB & ĐỘ TIN CẬY RABBITMQ -->
# 14. Fanout Pub/Sub & Bốn (4) Trụ Cột Tin Cậy Của RabbitMQ <span class="tag-theory">[LÝ THUYẾT]</span>
### Bác bỏ định kiến sai lầm: "Bất đồng bộ là dễ mất tin" $ightarrow$ An toàn vượt trội!

* **Fanout Exchange (Publish/Subscribe):** **BỎ QUA ROUTING KEY!** Khi có sự kiện `OrderCreated`, Exchange tự động nhân bản thông điệp thành 2 bản và gửi đồng thời tới `inventory.order-created` và `notification.order-created`.
* **Bốn (4) Trụ Cột An Toàn Tuyệt Đối (Zero Message Loss):**
  1. **Queue Durability (`durable = true`):** Hàng đợi được lưu trên đĩa, sống sót qua broker restart.
  2. **Message Persistence (`delivery_mode = 2`):** Thông điệp được ghi vào Disk Write-Ahead Log.
  3. **Publisher Confirms:** Phía gửi nhận tín hiệu xác nhận ghi đĩa từ Broker mới báo thành công.
  4. **Manual Consumer ACK (`autoAck = false`):** Xử lý nghiệp vụ và commit CSDL xong mới gửi `basicAck`.

```
[Producer] --Publish(delivery_mode=2)--> [RabbitMQ (Disk Log)] --Deliver--> [Consumer]
                                                    ^                          |
                                                    | (Nếu Crash trước ACK)    v (Commit DB OK)
                                                    +------ Requeue <------- basicAck
```

<!-- Speaker Notes:
RabbitMQ đảm bảo an toàn dữ liệu qua 4 trụ cột: Queue Durable trên đĩa, Message Persistent, Publisher Confirms và Consumer Manual ACK.
Worker xử lý xong xuôi mới gửi basicAck. Nếu worker bị mất điện giữa chừng, tin nhắn tự động được requeue lại!
-->

---

<!-- SLIDE 15: DEMO 1 - CẤU TRÚC MODULAR MONOLITH -->
# 15. DEMO 1: Cấu Trúc Modular Monolith & 3 Schema CSDL <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Kiểm chứng thực tế: 3 module độc lập trên PostgreSQL 16 & DbContext internal

* **Kiến trúc giải pháp (.NET 10 & PostgreSQL 16):**
  * `Order`: Quản lý đơn hàng, sở hữu schema `order`.
  * `Inventory`: Quản lý kho và tồn khả dụng, sở hữu schema `inventory`.
  * `Notification`: Gửi email/SMS mô phỏng, sở hữu schema `notification`.

```sql
-- Kiểm chứng 3 Schemas CSDL độc lập trong cùng 1 Server PostgreSQL:
SELECT schema_name FROM information_schema.schemata 
WHERE schema_name IN ('order', 'inventory', 'notification');
-- Kết quả trả về thực tế: 3 rows (inventory, order, notification)
```

<div class="highlight">
✔ <b>Kỷ luật kiến trúc:</b> 100% DbContext là <code>internal</code>, cấm tiệt truy cập chéo CSDL. Order chỉ phụ thuộc interface trừu tượng <code>IInventoryService</code> trong <code>Inventory.Contracts</code>.
</div>

<!-- Speaker Notes:
Bắt đầu phần Demo thực tế:
Demo 1 chứng minh cấu trúc Modular Monolith với 3 module riêng biệt và 3 schema CSDL độc lập trên PostgreSQL 16.
DbContext được khai báo internal, không có bất kỳ câu lệnh SQL JOIN chéo bảng nào.
-->

---

<!-- SLIDE 16: DEMO 2 - GIAO TIẾP ĐỒNG BỘ: ĐỦ HÀNG VS HẾT HÀNG -->
# 16. DEMO 2: Giao Tiếp Đồng Bộ — Đủ Hàng vs Hết Hàng <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Kiểm chứng quy trình: Kiểm tra – Giữ hàng nguyên tử (Reserve) – Chống bán vượt tồn

* **Tình huống 2.1: Đủ hàng (2 chiếc Laptop Gaming Pro 16)**
  * Order gọi đồng bộ `IInventoryService.ReserveAsync` $ightarrow$ Kho đủ hàng và tạm giữ 2 chiếc.
  * Phản hồi: **HTTP 201 Created** | Trạng thái `Confirmed` | Tổng tiền 50.000.000 VNĐ.
* **Tình huống 2.2: Hết hàng (1 chiếc Chuột RGB tồn kho = 0)**
  * Order gọi đồng bộ `IInventoryService.ReserveAsync` $ightarrow$ Kho báo tồn không đủ.
  * Phản hồi: **HTTP 409 Conflict** | Trạng thái `Rejected` | Thông báo: *"Tồn kho khả dụng không đủ"*.

```sql
-- Lệnh SQL Atomic Reserve chống bán vượt tồn thực thi trên PostgreSQL:
UPDATE inventory.stocks
SET available_quantity = available_quantity - @qty, reserved_quantity = reserved_quantity + @qty
WHERE product_id = @productId AND available_quantity >= @qty;
```

<!-- Speaker Notes:
Demo 2 kiểm chứng giao tiếp đồng bộ kiểm tra kho trước khi chốt đơn:
Đủ hàng -> 201 Created. Hết hàng -> 409 Conflict.
Cơ chế Reserve nguyên tử bằng câu lệnh SQL UPDATE có điều kiện available >= qty giúp chống bán vượt tồn tuyệt đối.
-->

---

<!-- SLIDE 17: DEMO 3 - BẤT ĐỒNG BỘ RABBITMQ & CONSUMER OFFLINE -->
# 17. DEMO 3: RabbitMQ Thật & Kịch Bản Consumer Offline <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Chứng minh trực quan: Tắt hoàn toàn Consumer thì thông điệp vẫn nằm chờ an toàn trên đĩa!

* **Quy trình thực nghiệm Consumer Offline trong Demo:**
  * **Bước 1:** Tắt Consumer Notification: `POST /demo/consumers/notification/stop` $ightarrow$ Trạng thái `OFFLINE`.
  * **Bước 2:** Khách đặt mua thêm 1 Laptop $ightarrow$ Server trả về **HTTP 201 Created** ngay tức thì (< 50ms)!
  * **Bước 3:** Kiểm tra hàng đợi RabbitMQ $ightarrow$ **Queue Depth = 1 (Ready = 1)** trên đĩa cứng!
  * **Bước 4:** Bật lại Consumer Notification: `POST /demo/consumers/notification/start` $ightarrow$ **Queue Depth về 0**, email mô phỏng được ghi vào CSDL!

```
[BẬT]  OrderCreated -> RabbitMQ -> Notification Consumer -> Gửi Mail -> basicAck (Ready = 0)
[TẮT]  OrderCreated -> RabbitMQ (Lưu trên đĩa cứng: Ready = 1) -> [Consumer đang OFFLINE]
[BẬT LẠI] Consumer Reconnect -> Kéo tin tồn đọng -> Gửi Mail -> basicAck (Ready = 0)
```

<!-- Speaker Notes:
Demo 3 là bằng chứng trực quan đanh thép về tính tin cậy của RabbitMQ:
Khi tắt Notification service, đơn hàng vẫn tạo thành công. Tin nhắn nằm chờ trên đĩa (Ready=1).
Khi bật lại service, tin nhắn được tiêu thụ ngay và trả hàng đợi về 0. Dữ liệu không hề bị mất!
-->

---

<!-- SLIDE 18: RACE CONDITION & BỘ KIỂM THỬ XUNIT -->
# 18. Kiểm Chứng Tranh Chấp Đồng Thời (`race.ps1`) & xUnit <span class="tag-ext">[HƯỚNG MỞ RỘNG]</span>
### Kịch bản thực tế: "Hai khách hàng cùng tranh mua chiếc Laptop cuối cùng"

* **Thực nghiệm script `demo/race.ps1` (Bắn 2 request song song):**
  * Tồn kho ban đầu của *Laptop Gaming Cuối Cùng*: **duy nhất 1 chiếc**.
  * Hai tiến trình đồng thời gửi request đặt mua tại cùng một thời điểm:
    * **Khách hàng 1:** Nhận **HTTP 201 Created** (Mua thành công).
    * **Khách hàng 2:** Nhận **HTTP 409 Conflict** (Báo hết hàng).
    * Tồn kho sau giao dịch: Khả dụng = `0`, Đã bán = `1`. Không bao giờ bị âm kho!

```text
==========================================================================
✔ XÁC NHẬN THÀNH CÔNG: Chống bán vượt tồn hoạt động 100% chuẩn xác! (Đúng 1x201 và 1x409)
==========================================================================
```

* **Bộ 5 Tests tự động xUnit (`ModularMonolith.Tests`):** Đủ hàng, Hết hàng, Tranh chấp song song, NetArchTest ranh giới kiến trúc, và DbContext internal $ightarrow$ **Passed 5/5 tests!**

<!-- Speaker Notes:
Slide mở rộng: Tranh chấp đồng thời mua chiếc laptop cuối cùng.
Script race.ps1 bắn 2 request cùng lúc: kết quả đúng 1 khách được 201 và 1 khách nhận 409.
Bộ test xUnit kiểm chứng tự động toàn bộ luồng trong 4 giây.
-->

---

<!-- SLIDE 19: TỔNG KẾT & BÀI HỌC CỐT LÕI -->
# 19. Tổng Kết Toàn Nhóm: Thế "Kiềng 3 Chân" & Bài Học <span class="tag-ext">[HƯỚNG MỞ RỘNG]</span>
### Mối quan hệ biện chứng ba chiều: Kiến trúc $\longleftrightarrow$ Ranh giới $\longleftrightarrow$ Giao tiếp

```
                    TRỤ CỘT 1: KIẾN TRÚC TỔNG THỂ (TV1)
                     * Chiến lược MonolithFirst & Kiến trúc lai
                     * Modular Monolith là điểm cân bằng tối ưu
                                    | (Định hình tư duy phân rã)
                                    v
                    TRỤ CỘT 2: RANH GIỚI NGHIỆP VỤ (TV2)
                     * Bounded Context, Bộ 4 câu hỏi vàng
                     * 4 Kỷ luật cô lập CSDL: CẤM SQL JOIN CHÉO
                                    | (Cấm JOIN chéo -> Cần giao tiếp mới)
                                    v
                    TRỤ CỘT 3: CƠ CHẾ GIAO TIẾP (TV3)
                     * Đồng bộ (Sync): Kiểm tra kho trước khi chốt đơn
                     * Bất đồng bộ (RabbitMQ): Phát sự kiện chạy ngầm
```

<div class="highlight">
🎯 <b>Ba Bài Học Đắt Giá:</b>
<br>1. <b>Phù hợp hơn hoàn hảo:</b> Microservices không phải đích đến bắt buộc; Modular Monolith là điểm ngọt ngào cho đa số doanh nghiệp.
<br>2. <b>Kỷ luật ranh giới là số 1:</b> Giữ vững kỷ luật schema CSDL thì mới có đường lui tiến hóa lên Microservice.
<br>3. <b>Kết hợp thông minh:</b> Dùng Đồng bộ cho quyết định tức thì, và dùng Bất đồng bộ cho mọi tác vụ chạy ngầm!
</div>

<!-- Speaker Notes:
Tổng kết: Toàn bộ bài báo cáo là một thể thống nhất hữu cơ:
TV1 định hướng chiến lược kiến trúc; TV2 phân rã ranh giới và cô lập CSDL; TV3 kết nối bằng Sync kiểm tra kho và Async RabbitMQ.
Ba bài học cốt lõi này là hành trang quý giá cho mỗi kỹ sư phần mềm hiện đại.
Xin chân thành cảm ơn Thầy và các bạn đã theo dõi bài báo cáo của nhóm!
-->