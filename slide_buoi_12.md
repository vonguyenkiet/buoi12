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
    font-size: 21.5px;
    padding-top: 50px;
    padding-bottom: 28px;
    padding-left: 48px;
    padding-right: 48px;
    color: #1e293b;
    background-color: #ffffff;
    line-height: 1.45;
  }
  header {
    top: 16px;
    left: 48px;
    font-size: 12px;
    color: #94a3b8;
    font-weight: 500;
  }
  footer {
    bottom: 14px;
    left: 48px;
    right: 48px;
    font-size: 11px;
    color: #94a3b8;
  }
  h1 {
    color: #0f172a;
    font-size: 26px;
    font-weight: 700;
    margin-bottom: 12px;
    padding-bottom: 5px;
    border-bottom: 1.5px solid #e2e8f0;
    letter-spacing: -0.02em;
    line-height: 1.3;
  }
  h2 {
    color: #1e40af;
    font-size: 20px;
    font-weight: 600;
    margin-bottom: 10px;
  }
  h3 {
    color: #334155;
    font-size: 18px;
    font-weight: 600;
    margin-bottom: 6px;
  }
  .tag-theory {
    background-color: #eff6ff;
    color: #1d4ed8;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 13px;
    font-weight: 700;
    vertical-align: middle;
  }
  .tag-demo {
    background-color: #f0fdf4;
    color: #15803d;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 13px;
    font-weight: 700;
    vertical-align: middle;
  }
  .tag-ext {
    background-color: #fefce8;
    color: #a16207;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 13px;
    font-weight: 700;
    vertical-align: middle;
  }
  .tag-app {
    background-color: #f1f5f9;
    color: #475569;
    padding: 3px 8px;
    border-radius: 4px;
    font-size: 13px;
    font-weight: 700;
    vertical-align: middle;
  }
  .card-order {
    background-color: #eff6ff;
    border: 1px solid #bfdbfe;
    border-left: 5px solid #2563eb;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .card-inv {
    background-color: #f0fdf4;
    border: 1px solid #bbf7d0;
    border-left: 5px solid #16a34a;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .card-notif {
    background-color: #fff7ed;
    border: 1px solid #fed7aa;
    border-left: 5px solid #ea580c;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .card-neutral {
    background-color: #f8fafc;
    border: 1px solid #e2e8f0;
    border-left: 5px solid #64748b;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .warning-box {
    background-color: #fffdf5;
    padding: 9px 13px;
    border: 1px solid #fef3c7;
    border-left: 5px solid #d97706;
    border-radius: 6px;
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
    padding: 8px 10px;
    font-size: 13px;
    line-height: 1.25;
    color: #0f172a;
  }
  code {
    font-family: 'Consolas', 'Fira Code', 'Courier New', monospace;
    color: #0f172a;
  }
---

<!-- SLIDE 1: TIÊU ĐỀ -->
# Modular Monolith: Tòa Nhà Chung Chia Phòng Rõ Ràng Để Dễ Dàng Tiến Hóa
## Nghiên cứu Lý thuyết & Thực nghiệm Chuyên đề — Buổi 12

<div class="card-neutral" style="margin-top: 15px;">
<b>Học phần:</b> Phát triển Hệ thống Server Nâng cao &nbsp;|&nbsp; <b>Bộ môn:</b> Kỹ thuật Phần mềm<br>
<b>Nhóm thực hiện:</b> Nhóm nghiên cứu Lý thuyết Buổi 12<br>
• <b>Thành viên 1 (TV1):</b> Kiến trúc tổng thể, Tiêu chí lựa chọn & Lộ trình 4 bước tiến hóa<br>
• <b>Thành viên 2 (TV2):</b> Tư duy phân rã nghiệp vụ (DDD), Bounded Context & 10 Module<br>
• <b>Thành viên 3 (TV3):</b> Huyết mạch giao tiếp Đồng bộ (Sync) & Bất đồng bộ (RabbitMQ)
</div>

<div class="highlight" style="margin-top: 10px;">
📖 <b>Câu chuyện xuyên suốt bài học:</b> <i>"Khách hàng bấm mua chiếc laptop cuối cùng của cửa hàng trong giờ cao điểm Flash Sale".</i>
</div>

<!-- Footer: Nhóm trưởng trình bày -->

<!-- Speaker Notes:
Kính chào Thầy và các bạn sinh viên. Hôm nay nhóm xin trân trọng báo cáo chuyên đề Buổi 12: "Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservice".
Thay vì tiếp cận bằng những định nghĩa khô khan, bài thuyết trình hôm nay sẽ dẫn dắt Thầy và các bạn qua câu chuyện thực tế: Khách hàng bấm mua chiếc laptop cuối cùng của cửa hàng.
Hệ thống của chúng ta gồm 3 nhân vật chính đại diện cho 3 phòng ban: Phòng Đơn hàng, Phòng Kho, và Phòng Thông báo.
Để hiểu cách 3 phòng ban này phối hợp nhịp nhàng, chúng ta hãy cùng bắt đầu với những câu hỏi hóc búa mà hệ thống phải đối mặt ngay sau đây.
-->

---

<!-- SLIDE 2: CÂU CHUYỆN MỞ ĐẦU -->
# Khách Bấm Mua Chiếc Laptop Cuối Cùng: Ba Câu Hỏi Lớn <span class="tag-theory">[LÝ THUYẾT]</span>
### Tình huống thực tế: Đơn hàng, Kho và Thông báo phối hợp như thế nào?

<div class="grid-3">
<div class="card-inv">

### 📦 1. Còn hàng không?
* Phòng **Kho (Inventory)** phải kiểm tra ngay tức thì.
* Khóa giữ hàng nguyên tử trên sổ sách.
* Nếu hết hàng: Từ chối ngay.

</div>
<div class="card-order">

### 📝 2. Ai chốt đơn hàng?
* Phòng **Đơn hàng (Order)** lập hợp đồng giao dịch.
* Lưu bản chụp (Snapshot) giá bán lúc khách bấm mua.
* Xác nhận đơn thành công.

</div>
<div class="card-notif">

### 📬 3. Ai báo cho khách?
* Phòng **Thông báo (Notification)** nhận tin từ hòm thư.
* Tự động gửi Email & SMS xác nhận (mô phỏng).
* Chạy ngầm, không bắt khách chờ.

</div>
</div>

<div class="warning-box">
⚡ <b>Bài toán cốt lõi:</b> Nếu 2 khách cùng bấm mua chiếc laptop cuối cùng vào cùng một phần nghìn giây, làm sao để đúng 1 người mua được và hệ thống không bao giờ bị âm kho?
</div>

<!-- Footer: Nhóm trưởng trình bày -->

<!-- Speaker Notes:
Hãy hình dung tình huống thực tế: Cửa hàng chỉ còn đúng 1 chiếc Laptop Gaming duy nhất, và khách hàng bấm nút Đặt hàng.
Lúc này, hệ thống máy chủ phải trả lời 3 câu hỏi sống còn:
Thứ nhất: Kho còn hàng hay không? Phòng Đơn hàng phải liên hệ ngay với Phòng Kho để kiểm tra và giữ hàng.
Thứ hai: Sau khi giữ được hàng, Phòng Đơn hàng chốt hợp đồng và ghi nhận đơn thành công.
Thứ ba: Làm sao để gửi email cho khách mà không làm khách phải đứng chờ màn hình xoay vòng?
Và bài toán hóc búa nhất: Nếu 2 người cùng tranh mua chiếc laptop cuối cùng, kiến trúc nào sẽ bảo vệ kho không bị bán âm? Xin mời bạn Thành viên 1 mở đầu với các lựa chọn kiến trúc.
-->

---

<!-- SLIDE 3: BA CÁCH XÂY DỰNG CỬA HÀNG -->
# Ba Cách Xây Dựng Cửa Hàng: Quán Nhỏ, Tòa Nhà Chia Phòng Và Chuỗi Chi Nhánh <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 1 — So sánh 3 triết lý kiến trúc phần mềm phổ biến hiện nay

| Tiêu chí | 🏠 Quán nhỏ (Monolithic) | 🏢 Tòa nhà chia phòng (Modular Monolith) | 🌐 Chuỗi chi nhánh (Microservices) |
|---|---|---|---|
| **Ví von đời thường** | Một người làm hết mọi việc từ bán đến kho | Tòa nhà chung 1 cổng, chia 3 phòng có khóa riêng | Mỗi bộ phận thuê một tòa nhà riêng ngoài phố |
| **Mã nguồn (Codebase)** | 1 Repository duy nhất | 1 Repository duy nhất | Nhiều Repository riêng biệt (hoặc Monorepo) |
| **Đơn vị Deployment** | 1 Process / 1 file thực thi duy nhất | **1 Process / 1 file thực thi duy nhất** | Hàng chục Containers / Pods độc lập |
| **Quyền sở hữu CSDL** | 1 Database chung, query tự do | **1 Database chung, CÔ LẬP THEO 3 SCHEMAS** | Database-per-Service (mỗi service 1 CSDL riêng) |

<div class="highlight">
🎯 <b>Đặc điểm cốt lõi:</b> Modular Monolith vận hành gọn nhẹ y hệt Monolith truyền thống (1 process, 1 database), nhưng tổ chức mã nguồn có tính kỷ luật cao như Microservices.
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Chào Thầy và các bạn, tôi là Thành viên 1. Khi nhận nhiệm vụ xây dựng hệ thống bán hàng, chúng ta có 3 cách tiếp cận:
Cách 1 như mở một quán nhỏ: một người làm hết mọi việc, code chung một chỗ, dùng chung một database.
Cách 2 như một tòa nhà chung nhưng chia phòng rõ ràng: một tiến trình duy nhất, nhưng phòng Đơn hàng, phòng Kho, phòng Thông báo được ngăn cách bằng cửa khóa riêng và schema CSDL riêng.
Cách 3 như chuỗi chi nhánh: mỗi phòng ban dọn ra thuê hẳn một tòa nhà riêng ngoài phố, kết nối với nhau qua mạng viễn thông.
Mỗi cách đều có cái giá phải trả, chúng ta hãy cùng so sánh 2 thái cực ở slide tiếp theo.
-->

---

<!-- SLIDE 4: HAI THÁI CỰC KIẾN TRÚC -->
# Hai Thái Cực Kiến Trúc: Đơn Khối Truyền Thống Và Vi Dịch Vụ Đều Có Giá Phải Trả <span class="tag-theory">[LÝ THUYẾT]</span>
### Không có giải pháp hoàn hảo tuyệt đối — Mọi kiến trúc đều là sự đánh đổi

<div class="grid-2">
<div class="card-neutral">

### 🏢 Monolithic Truyền Thống
* **Ưu điểm thực tế:**
  * Đơn giản để phát triển, build, test và deploy.
  * Rất phù hợp cho nhóm nhỏ và giai đoạn đầu.
  * Hiệu năng cao nhờ gọi hàm trực tiếp trong RAM.
* **Cái giá phải trả khi phình to:**
  * Codebase rối dần thành mớ bòng bong (**Spaghetti**).
  * Khó scale riêng từng phần chịu tải (như Kho hàng).
  * Một lỗi nhỏ ở một chỗ có thể làm sập cả hệ thống.
  * Nhiều nhóm cùng sửa mã nguồn dễ xung đột dẫm chân.

</div>
<div class="card-neutral">

### 🌐 Microservices Phân Tán
* **Ưu điểm chiến lược:**
  * Scale độc lập đúng phần cần (ví dụ nhân bản Kho hàng).
  * Các nhóm làm việc song song không phụ thuộc nhau.
  * Dễ thay thế hoặc nâng cấp công nghệ từng phần.
* **Khoản thuế phân tán (Distributed Tax):**
  * Phức tạp vận hành: Cần CI/CD riêng, gateway, log tập trung.
  * Độ trễ mạng và rủi ro rớt gói tin giữa các service.
  * Giao dịch trải dài nhiều service rất khó xử lý (mất ACID).
  * Đòi hỏi đội ngũ DevOps đủ mạnh và hạ tầng đủ lớn.

</div>
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Trong kỹ nghệ phần mềm không có bữa trưa nào miễn phí.
Monolith truyền thống rất ngọt ngào lúc ban đầu: một sinh viên chỉ cần nhấn F5 là chạy được toàn bộ app. Nhưng sau vài năm, nó thoái hóa thành một bãi rác spaghetti code, việc sửa code giỏ hàng có thể làm treo cả hệ thống thanh toán.
Nhảy sang Microservices thì sao? Bạn được quyền scale độc lập từng service, nhưng bạn phải gánh một "khoản thuế phân tán" khổng lồ: chi phí máy chủ tăng vọt, mất đi giao dịch ACID, và nếu mạng chập chờn 1 giây, lỗi sẽ lan truyền khắp nơi.
Đó là lý do Modular Monolithic ra đời như một điểm cân bằng tối ưu.
-->

---

<!-- SLIDE 5: MODULAR MONOLITHIC NẰM Ở GIỮA -->
# Modular Monolithic Nằm Ở Điểm Cân Bằng: Vận Hành Đơn Khối, Kỷ Luật Vi Dịch Vụ <span class="tag-theory">[LÝ THUYẾT]</span>
### Tòa nhà một cổng, chia phòng khép kín — Giải pháp ngọt ngào cho phần lớn ứng dụng

<div class="highlight">
<b>Định nghĩa:</b> Vẫn là <b>một codebase, một process, một đơn vị deploy</b> (đơn giản như Monolith), nhưng code được <b>tổ chức chặt chẽ thành các module độc lập</b> theo ranh giới nghiệp vụ, giao tiếp qua Interface thay vì gọi chéo tùy tiện.
</div>

```
+-------------------------------------------------------------------------+
|                        MODULAR MONOLITH RUNTIME                         |
|                       (1 Tiến trình thực thi duy nhất)                  |
|                                                                         |
|   [ ORDER MODULE ]             In-Memory Call           [ INVENTORY ]   |
|   - Order.Domain            ======================>     - StockItem     |
|   - Order.Application         (IInventoryService)       - CheckStock    |
|   - Internal DbContext                                  - Internal Db   |
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

* **Ưu thế vượt trội:** Vận hành cực rẻ (1 pipeline, 1 lần deploy), gọi hàm in-process trong RAM siêu tốc, nhưng các module hoàn toàn độc lập CSDL.

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Modular Monolithic chính là điểm giao thoa hoàn hảo giữa hai thế giới:
Về mặt vận hành, nó là một Monolith: chỉ 1 tiến trình chạy trên 1 máy chủ, 1 pipeline CI/CD, chi phí duy trì hàng tháng cực kỳ tiết kiệm.
Về mặt tổ chức code, nó có kỷ luật của Microservices: Phòng Đơn hàng muốn kiểm tra kho phải đi qua interface IInventoryService.
Ở tầng CSDL, cả 2 module có thể nằm chung 1 máy chủ PostgreSQL nhưng chia thành 2 schema riêng biệt: schema order và schema inventory.
Tuyệt đối không có lệnh SQL JOIN xuyên schema! Nhờ đó, việc bóc tách sau này trở nên vô cùng dễ dàng.
-->

---

<!-- SLIDE 6: MA TRẬN ĐỐI SÁNH KỸ THUẬT -->
# Ma Trận Đối Sánh Kỹ Thuật Chuyên Sâu Giữa Ba Mô Hình Kiến Trúc <span class="tag-theory">[LÝ THUYẾT]</span>
### Đánh giá toàn diện 8 tiêu chí cốt lõi trong kỹ nghệ phần mềm máy chủ

| Tiêu chí kỹ thuật | Monolithic truyền thống | Modular Monolithic | Microservices Architecture |
|---|---|---|---|
| **1. Codebase** | 1 Repository | 1 Repository (chia thư mục/project) | Đa Repository hoặc Monorepo |
| **2. Tiến trình (Process)** | 1 Process duy nhất | **1 Process duy nhất** | Đa Process phân tán trên mạng |
| **3. Đơn vị Deployment** | 1 File build / Container | **1 File build / Container** | Hàng chục Container / Pods riêng |
| **4. Quyền sở hữu CSDL** | Chung 1 CSDL, JOIN tự do | **1 CSDL vật lý, tách riêng Schema** | Tách riêng CSDL vật lý (hoặc schema) |
| **5. Phương thức giao tiếp**| Gọi hàm trực tiếp | **Qua Public Interface trong RAM** | Lời gọi mạng (REST, gRPC, MQ) |
| **6. Khả năng mở rộng (Scale)**| Chỉ scale toàn bộ khối | Scale toàn bộ khối (hoặc tách lẻ) | **Scale độc lập từng microservice** |
| **7. Độ phức tạp DevOps** | Rất thấp (1 máy chủ) | **Thấp (1 pipeline đơn giản)** | **Rất cao (K8s, Gateway, Tracing)** |
| **8. Chi phí bóc tách sau này**| Cực kỳ khó khăn (rối code)| **Rất thuận lợi (nhờ Seam có sẵn)** | Không cần bóc tách (đã tách sẵn) |

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Bảng ma trận 8 tiêu chí kỹ thuật này là bằng chứng rõ nhất cho hội đồng:
Nhìn vào bảng, ta thấy Modular Monolith thừa hưởng trọn vẹn sự đơn giản của Monolith ở 4 tiêu chí đầu: 1 repo, 1 process, 1 container deploy, và độ phức tạp DevOps rất thấp.
Nhưng ở tiêu chí số 4, 5 và 8, nó lại sở hữu phẩm chất của Microservices: dữ liệu phân tách rõ ràng theo schema, giao tiếp qua interface trừu tượng, và chi phí bóc tách sau này cực kỳ thấp.
Đây chính là bệ phóng lý tưởng cho mọi doanh nghiệp trước khi nghĩ đến chuyện chia nhỏ hệ thống.
-->

---

<!-- SLIDE 7: BA TIÊU CHÍ CHỌN KIẾN TRÚC -->
# Ba Tiêu Chí Chọn Kiến Trúc: Đội Ngũ, Năng Lực DevOps Và Giai Đoạn Sản Phẩm <span class="tag-theory">[LÝ THUYẾT]</span>
### Quyết định dựa trên năng lực thực tế — Cảnh giác với cái bẫy trào lưu công nghệ

<div class="grid-3">
<div class="card-neutral">

### 👥 1. Quy Mô Đội Ngũ
*(Định luật Conway)*
* **Đội nhỏ (vài người, nhóm SV):** Modular Monolith dễ quản lý hơn nhiều lần.
* **Nhiều team độc lập:** Microservices giúp các nhóm deploy song song không đụng nhau.
* *Inverse Conway:* Tổ chức team trước khi chia service.

</div>
<div class="card-neutral">

### 🛠️ 2. Hạ Tầng & DevOps
*(Gánh Nặng Vận Hành)*
* Microservices đòi hỏi hạ tầng và đội ngũ DevOps đủ mạnh.
* Doanh nghiệp nhỏ thiếu DevOps rất dễ "chết" vì gánh nặng vận hành trước khi kịp có lợi ích scale!
* Modular Monolith: 1 pipeline duy nhất.

</div>
<div class="card-neutral">

### ⏱️ 3. Giai Đoạn Sản Phẩm
*(Mức Độ Ổn Định)*
* **Sản phẩm mới (MVP):** Ranh giới nghiệp vụ chưa ổn định 
 →  Modular Monolith giúp tái cấu trúc cực nhanh.
* **Đã trưởng thành:** Biết rõ phần cần scale riêng (ví dụ Kho mùa sale) 
 →  Tách đúng phần đó.

</div>
</div>

<div class="highlight">
💡 <b>Định luật Conway (Conway's Law):</b> Cấu trúc hệ thống phần mềm luôn phản chiếu cơ cấu giao tiếp của tổ chức. Đừng phân tán hệ thống nếu tổ chức của bạn chưa phân tán!
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Làm thế nào để chọn đúng kiến trúc mà không bị lóa mắt bởi các từ khóa thời thượng? Chúng ta căn cứ vào 3 tiêu chí:
Thứ nhất: Định luật Conway. Cấu trúc code phản ánh cấu trúc tổ chức. Một nhóm sinh viên 3 người mà chia 10 microservices thì mỗi người phải ôm 3 repo, tự làm khổ mình.
Thứ hai: Hạ tầng và DevOps. Microservices đòi hỏi K8s, tracing tập trung. Nếu thiếu nguồn lực này, doanh nghiệp sẽ chết vì kiệt sức vận hành trước khi thấy được lợi ích.
Thứ ba: Giai đoạn sản phẩm. Khi làm MVP, nghiệp vụ đổi từng tuần. Sửa code trong Monolith mất 5 phút refactor; đổi API qua mạng giữa 10 service mất cả tuần lễ!
-->

---

<!-- SLIDE 8: MONOLITHFIRST VÀ KIẾN TRÚC LAI -->
# Bắt Đầu Bằng Modular Monolith Và Tồn Tại Kiến Trúc Lai Trong Doanh Nghiệp <span class="tag-theory">[LÝ THUYẾT]</span>
### Nguyên tắc MonolithFirst (Martin Fowler) — Phá vỡ tư duy nhị nguyên cực đoan

* **Nguyên tắc "MonolithFirst" (Martin Fowler - 2015):** Không bắt đầu bằng Microservices ngay từ ngày đầu! Luôn bắt đầu với Modular Monolithic, chỉ tách một module ra thành service độc lập khi có **lý do rõ ràng** (cần scale riêng, cần công nghệ khác, bảo mật/compliance). Không tách "cho oai", không tách hết một lượt.
* **Hiểu lầm phổ biến:** Nghĩ rằng hệ thống phân tán là phải "100% Microservices".
* **Thực tế phổ biến:** **Kiến trúc Lai (Hybrid Architecture)** — Giữ 80–90% trong Monolith lõi, chỉ tách các vệ tinh đặc thù.

<div class="grid-2">
<div class="card-inv">

### 📦 Case Study: Tách Module Inventory
* **Lý do tách:** Mùa Flash Sale, hàng vạn khách cùng tranh mua chiếc laptop cuối cùng 
 →  Kho cần scale từ 2 lên 20 Pods chịu tải.

</div>
<div class="card-notif">

### 💳 Case Study: Tách Module Payment
* **Lý do tách:** Yêu cầu tuân thủ an ninh tài chính quốc tế PCI-DSS 
 →  Cô lập máy chủ thanh toán trong vùng mạng riêng để giảm chi phí kiểm toán.

</div>
</div>

<div class="warning-box" style="text-align: center; font-weight: bold;">
⚠️ LƯU Ý BẮT BUỘC: "Microservices KHÔNG PHẢI là đích đến bắt buộc của mọi hệ thống!"
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Martin Fowler đã đúc kết nguyên tắc MonolithFirst: Hầu hết các hệ thống vi dịch vụ thành công đều bắt đầu từ một Monolith được thiết kế tốt rồi mới chia nhỏ.
Và trong thực tế, không có chuyện bắt buộc phải 100% Microservices. Mô hình Kiến trúc Lai (Hybrid) là phổ biến nhất:
Doanh nghiệp giữ phần lớn nghiệp vụ trong Modular Monolith lõi, chỉ tách đúng 2 module đặc thù:
1. Tách module Kho (Inventory) để scale riêng vào mùa Flash Sale khi khách tranh mua chiếc laptop cuối cùng.
2. Tách module Thanh toán (Payment) để đạt chứng chỉ an ninh PCI-DSS mà không phải kiểm toán cả triệu dòng code của khối Monolith.
Hãy luôn nhớ: Microservices không phải là đích đến bắt buộc!
-->

---

<!-- SLIDE 9: LỘ TRÌNH TIẾN HÓA 4 BƯỚC -->
# Lộ Trình Tiến Hóa 4 Bước: Bóc Tách Module Mà Không Phải Viết Lại Logic <span class="tag-theory">[LÝ THUYẾT]</span>
### Nhờ đã có sẵn Schema riêng và Public Interface từ ngày đầu, việc tách module chỉ cần 4 bước

```
   [ BƯỚC 1: TÁCH DATABASE RIÊNG ]        -> Tách Schema 'inventory' sang máy chủ DB vật lý riêng
                 |
   [ BƯỚC 2: ĐỔI INTERFACE SANG NETWORK ]  -> Thay lời gọi in-process bằng Network Call (REST/gRPC/RabbitMQ)
                 |
   [ BƯỚC 3: ĐÓNG GÓI DOCKER RIÊNG ]       -> Đóng gói module thành Docker Image riêng (< 150MB, kế thừa B6, 7)
                 |
   [ BƯỚC 4: PIPELINE CI/CD RIÊNG ]        -> Tạo pipeline CI/CD riêng để deploy độc lập (kế thừa B10, 11)
```

* **Thực tế kỹ thuật khi bóc tách:**
  * **Mã nguồn nghiệp vụ lõi (Domain Logic) được giữ nguyên:** Nhờ nguyên lý Dependency Inversion, `OrderService` chỉ phụ thuộc vào `IInventoryService`, không đổi logic khi đổi cách gọi.
  * **Các thách thức mới phát sinh:** Cần xử lý độ trễ mạng (Network Latency), lỗi phân tán (Network Timeout, Circuit Breaker), và bài toán nhất quán dữ liệu phân tán.

<div class="highlight">
🔗 <b>Chuyển giao vai trò:</b> Để có được đường cắt interface và schema độc lập cho 4 bước này, <b>Tư duy Phân rã Nghiệp vụ DDD của Thành viên 2</b> chính là nền móng quyết định!
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Đây là slide chốt lại phần trình bày của TV1: Lộ trình 4 bước bóc tách một module.
Bước 1: Tách schema CSDL thành database riêng.
Bước 2: Thay lời gọi interface trong bộ nhớ bằng lời gọi qua mạng (REST, gRPC hoặc RabbitMQ).
Bước 3: Đóng gói Docker image riêng theo bài học Buổi 6, 7.
Bước 4: Tạo pipeline CI/CD riêng theo bài học Buổi 10, 11.
Nhờ tuân thủ kỷ luật Modular Monolith, code nghiệp vụ trừ kho của Inventory không phải viết lại từ đầu. Tuy nhiên, kỹ sư phải đối mặt với các bài toán mới như lỗi mạng và timeout.
Và làm thế nào để vạch đúng ranh giới cho từng module? Xin mời Thầy và các bạn cùng đến với phần của Thành viên 2!
-->

---

<!-- SLIDE 10: CHIA MODULE THEO NGHIỆP VỤ -->
# Chia Module Theo Chiều Dọc Nghiệp Vụ, Tuyệt Đối Không Chia Theo Cảm Tính ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 2 (Mục 2.2 — Trọng tâm khó nhất): Tư duy Domain Decomposition

<div class="grid-2">
<div class="card-neutral">

### ❌ Cắt Ngang Kỹ Thuật (Cảm Tính / Sai Lầm)
* Gom cụm theo tầng kỹ thuật: Thư mục `Controllers`, `Services`, `Repositories` dùng chung cho toàn bộ app.
* **Hậu quả thảm khốc:** Một tính năng Đơn hàng bị xé vụn ra 3 nơi; sửa công thức tính thuế phải sửa ở hàng loạt tầng; code bị coupling chằng chịt, sau này không thể bóc tách!

</div>
<div class="card-neutral">

###  Cắt Dọc Nghiệp Vụ (DDD Chuẩn)
* Áp dụng **Domain-Driven Design (DDD)** của Eric Evans: Phân rã bài toán lớn thành các **Bounded Context** độc lập.
* Mỗi module bao trọn một mảng nghiệp vụ khép kín từ tầng tiếp nhận (API), tầng điều phối (Application), tầng nghiệp vụ thuần (Domain) tới CSDL riêng.

</div>
</div>

```
MÔ HÌNH CHIA NGANG TRUYỀN THỐNG (SAI)         MÔ HÌNH CHIA DỌC MODULAR MONOLITH (ĐÚNG)
[ Controllers Layer (Chung toàn app) ]       +-----------------+ +-----------------+ +-----------------+
[ Services Layer    (Gọi chéo tùm lum) ]     | CATALOG MODULE  | |  ORDER MODULE   | |INVENTORY MODULE |
[ Repositories / DB (JOIN bảng vô tội vạ) ]  | - Controller    | | - Controller    | | - Controller    |
                                             | - Domain Logic  | | - Domain Logic  | | - Domain Logic  |
                                             | - Catalog Schema| | - Order Schema  | | - Inven Schema  |
                                             +-----------------+ +-----------------+ +-----------------+
```

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Kính thưa Thầy và các bạn, tôi là Thành viên 2, phụ trách mục 2.2 – phần được xem là trái tim và là phần khó nhất của buổi học hôm nay: Tư duy Phân rã Nghiệp vụ.
Viết code chia thư mục thì ai cũng làm được, nhưng biết cách vạch ra ranh giới đúng để sau này không bị dính chặt vào nhau chính là bản lĩnh của người làm kiến trúc.
Sai lầm phổ biến nhất của sinh viên là chia ngang kỹ thuật: tạo folder Controllers, Services, Repositories. Kết quả là tính năng Đơn hàng bị xé lẻ ra 3 nơi, sửa một tí là gãy nát khắp nơi.
Do đó, chúng ta bắt buộc phải chia dọc theo Domain-Driven Design: mỗi module là một vương quốc độc lập từ giao diện đến CSDL.
-->

---

<!-- SLIDE 11: BOUNDED CONTEXT VÀ UBIQUITOUS LANGUAGE -->
# Bounded Context: Cùng Chữ "Sản Phẩm", Mỗi Phòng Ban Hiểu Theo Một Nghĩa Riêng ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Ngôn ngữ chung (Ubiquitous Language) — Chấm dứt ảo tưởng về một Entity dùng chung

<div class="highlight">
<b>Quy tắc vàng DDD:</b> Một thuật ngữ chỉ có ý nghĩa chính xác, duy nhất bên trong một Bounded Context cụ thể. Gom chung tất cả vào 1 class sẽ tạo ra <b>God Class</b> phình to 70 thuộc tính!
</div>

* **Bốn (4) lăng kính khác nhau của thực thể "Product":**
  * **Catalog (Chào bán):** Tiêu đề, danh mục, hình ảnh đẹp, mô tả SEO, giá niêm yết 
 →  *Tối ưu hiển thị cho khách xem.*
  * **Inventory (Lưu kho SKU):** Mã vạch Barcode, vị trí kệ kho (Aisle/Bin), tồn khả dụng 
 →  *Kiểm soát số lượng thực tế.*
  * **Cart (Giỏ hàng):** Số lượng tạm thời khách nhặt vào giỏ 
 →  *Dữ liệu biến động nhanh trên Redis.*
  * **Order (Đơn hàng):** **Bản chụp bất biến (Immutable Snapshot)** giá và tên lúc bấm mua 
 →  *Ràng buộc pháp lý và hóa đơn.*

<div class="warning-box">
⚠️ <b>Câu chuyện chiếc laptop cuối cùng:</b> Khách bấm mua lúc giá 30 triệu; ngày mai Catalog tăng giá lên 35 triệu thì đơn hàng cũ <b>tuyệt đối không được phép đổi giá theo</b>! Đó là lý do Order phải lưu Snapshot độc lập, cấm SQL JOIN sang Catalog!
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Khái niệm cốt lõi nhất của DDD là Bounded Context và Ubiquitous Language. Đừng bao giờ ảo tưởng tạo ra một class Product duy nhất với 70 thuộc tính dùng chung cho cả công ty!
Hãy nhìn vào chiếc laptop cuối cùng của chúng ta:
Ở phòng Catalog, nó là hình ảnh đẹp mắt để chào bán.
Ở phòng Kho, nó chỉ là một mã SKU nằm ở kệ kho số mấy để nhân viên ra nhặt hàng.
Ở Giỏ hàng, nó chỉ là số lượng tạm thời khách nhặt vào giỏ.
Và quan trọng nhất: Ở phòng Đơn hàng, nó là một Bản chụp bất biến (Snapshot)! Nếu tuần sau shop tăng giá, đơn hàng đã chốt hôm nay tuyệt đối không được biến động theo. Đó là lý do Order phải lưu riêng, cấm tiệt SQL JOIN sang bảng của Catalog!
-->

---

<!-- SLIDE 12: BÀI TOÁN E-COMMERCE 10 NGHIỆP VỤ -->
# Bài Toán E-Commerce: Mười Nghiệp Vụ Trong Một Doanh Nghiệp Bán Hàng Trực Tuyến ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Phân loại 8 nghiệp vụ cốt lõi và 2 nghiệp vụ mở rộng theo yêu cầu học phần

<div class="grid-2">
<div>

### 📦 Tám (8) Nghiệp Vụ Lõi (Bắt Buộc Theo Đề)
* **1. Catalog:** Quản lý sản phẩm, danh mục, mô tả.
* **2. Cart:** Giỏ hàng tạm thời, tối ưu trải nghiệm.
* **3. Order:** Trái tim xử lý đơn hàng (Core Domain).
* **4. Payment:** Giao dịch thanh toán, cổng ngân hàng.
* **5. Inventory:** Quản lý tồn kho, vị trí kệ, SKU.
* **6. Shipping:** Quản lý giao vận và đối tác 3PL.
* **7. Identity:** Xác thực, tài khoản người dùng, token.
* **8. Notification:** Gửi thông báo Email, SMS cho khách.

</div>
<div>

### 🌟 Hai (2) Nghiệp Vụ Mở Rộng
* **9. Promotion (Khuyến mãi):**
  * Quản lý Voucher, mã giảm giá, luật Flash Sale.
  * Tốc độ thay đổi rất nhanh theo chiến dịch tiếp thị.
* **10. Review (Đánh giá):**
  * Đánh giá 1–5 sao, nhận xét sản phẩm của khách.
  * Nghiệp vụ CRUD đơn giản, độc lập.

<div class="highlight" style="margin-top: 10px;">
📌 <b>Ghi chú kiến trúc:</b> Promotion và Review là các mảng mở rộng; hoàn toàn có thể tổ chức thành các module độc lập tùy theo quy mô tổ chức.
</div>

</div>
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Trong bài toán Thương mại điện tử thực tế, chúng ta khảo sát 10 mảng nghiệp vụ:
8 module lõi bắt buộc theo tài liệu môn học gồm: Catalog, Cart, Order, Payment, Inventory, Shipping, Identity và Notification.
Hai module mở rộng là Promotion (khuyến mãi) và Review (đánh giá).
Mỗi nghiệp vụ này đều giải quyết một bài toán riêng biệt của doanh nghiệp bán hàng.
Nhưng làm sao để biết chắc chắn nghiệp vụ nào nên tách thành module riêng và nghiệp vụ nào nên gộp lại? Chúng ta cùng đến với Bộ 4 câu hỏi vàng ở slide tiếp theo.
-->

---

<!-- SLIDE 13: BỘ 4 CÂU HỎI VÀNG -->
# Bộ Bốn Câu Hỏi Vàng Để Quyết Định Ranh Giới Module Một Cách Khoa Học ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Khung đánh giá chuẩn mực — Chấm dứt hoàn toàn sự tranh cãi cảm tính giữa các kỹ sư

| # | Câu hỏi vàng | Bản chất kỹ thuật & Nghiệp vụ | Mục tiêu bảo vệ |
|---|---|---|---|
| **1** | **Ngôn ngữ nghiệp vụ riêng?** | Có hệ thống thuật ngữ, khái niệm đặc thù không? (Ví dụ: "SKU", "tồn khả dụng" khác "giỏ hàng", "voucher") | Chống ép khuôn mô hình (Model Smashing) |
| **2** | **Ranh giới tổ chức / phòng ban?** | Bộ phận nào chịu trách nhiệm trong thực tế? Kho vận khác Marketing khác Kế toán. (Định luật Conway) | Xác định rõ Product Owner chịu trách nhiệm |
| **3** | **Độc lập dữ liệu (No-JOIN)?** | Dữ liệu có thể đứng độc lập mà **KHÔNG CẦN viết SQL JOIN trực tiếp** xuyên suốt với module khác không? | Triệt tiêu phụ thuộc CSDL, sẵn sàng bóc tách |
| **4** | **Tốc độ thay đổi yêu cầu?** | Tần suất thay đổi có khác nhau không? (Promotion đổi theo tuần/ngày; Catalog ổn định theo tháng/năm) | Cô lập vùng biến động cao khỏi vùng ổn định |

<div class="highlight" style="margin-top: 10px;">
🎯 <b>Tiêu chuẩn vàng:</b> Nếu một nghiệp vụ trả lời "CÓ" cho cả 4 câu hỏi, đó là ứng viên hoàn hảo để đóng gói thành một Module độc lập vững chắc!
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Để không bị rơi vào tranh cãi cảm tính xem nên chia hệ thống thành 5 hay 10 module, tài liệu môn học Buổi 12 trang bị cho chúng ta Bộ 4 câu hỏi vàng:
1. Ngôn ngữ có riêng biệt không? Nếu kho dùng từ SKU, vị trí kệ hàng, trong khi marketing dùng từ voucher, giảm giá, thì đó là 2 ngôn ngữ riêng.
2. Phòng ban nào sở hữu trong tổ chức thật? Theo Định luật Conway, ranh giới tổ chức chính là ranh giới module.
3. Dữ liệu có đứng độc lập không cần SQL JOIN không? Đây là điều kiện sống còn để sau này tách CSDL ra riêng.
4. Tốc độ thay đổi thế nào? Tách phần biến động nhanh như Khuyến mãi ra khỏi phần ổn định như Quản lý sản phẩm để không làm lỗi dây chuyền.
-->

---

<!-- SLIDE 14: ÁP DỤNG BỘ 4 CÂU HỎI -->
# Áp Dụng Bộ 4 Câu Hỏi: Ranh Giới Module Không Có Một Đáp Án Tuyệt Đối Duy Nhất ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Khảo sát các module tiêu biểu — Tôn trọng tính biện chứng trong thiết kế hệ thống

| Nghiệp vụ tiêu biểu | 1. Ngôn ngữ riêng | 2. Phòng ban thực tế | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Kết luận đề xuất |
|---|---|---|---|---|:---:|
| **Inventory (Kho)** | SKU, Tồn khả dụng, Vị trí kệ | Phòng Kho vận | Cao: Quản lý tồn theo SKU | Thấp | **TÁCH RIÊNG (Lõi)** |
| **Promotion (Khuyến mãi)** | Voucher, Giảm giá, Quota | Phòng Marketing | Cao: Tính tiền giảm qua DTO | **RẤT CAO** | **TÁCH RIÊNG (Mở rộng)**|
| **Payment (Thanh toán)** | Giao dịch, Đối soát, Refund | Phòng Kế toán - Tài chính| Tuyệt đối: Lưu log đối soát | Thấp (PCI-DSS)| **TÁCH RIÊNG (Lõi)** |
| **Notification (Thông báo)** | Template, Kênh Email/SMS | Bộ phận CSKH | Tuyệt đối: Nhận payload từ Queue | Trung bình | **TÁCH RIÊNG (Lõi)** |

<div class="warning-box">
⚖️ <b>HỘP LƯU Ý BẮT BUỘC (Không có đáp án duy nhất):</b> Việc chia module có thể có nhiều cách hợp lý khác nhau tùy góc nhìn tổ chức. Ví dụ: <b>Cart và Order tách riêng hay gộp chung</b> đều được chấp nhận, miễn là giải thích được lý do căn cứ theo Bộ 4 câu hỏi trên! <i>(Ma trận đầy đủ 10 nghiệp vụ xem tại Phụ lục A).</i>
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Khi áp dụng Bộ 4 câu hỏi vào thực tế, chúng ta thấy rõ:
Inventory tách riêng vì ngôn ngữ kho vận đặc thù và chịu tải cao.
Promotion tách riêng vì Marketing đổi luật khuyến mãi liên tục, tách ra để bảo vệ các module khác.
Payment tách riêng vì liên quan đến chứng chỉ an toàn tài chính.
Và giảng viên chấm thi thường hỏi: "Cart và Order tách hay gộp?". Nhóm xin khẳng định: Ranh giới module không có đáp án tuyệt đối duy nhất!
Nếu nhóm sinh viên muốn làm nhanh, gộp Cart vào Order vẫn hợp lý. Nếu muốn tối ưu trải nghiệm giỏ hàng trên Redis, tách Cart riêng cũng hoàn toàn đúng đắn, miễn là giải thích được căn cứ theo 4 câu hỏi vàng!
-->

---

<!-- SLIDE 15: CẤU TRÚC CLEAN ARCHITECTURE NỘI BỘ -->
# Cấu Trúc Clean Architecture Nội Bộ: Mỗi Module Là Một Ứng Dụng Thu Nhỏ ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Kế thừa Buổi 4: Phân tầng nghiêm ngặt và đóng gói bằng từ khóa internal trong C#

<div class="grid-2">
<div>

### 🏗️ Bốn (4) Tầng Bên Trong Module
* **Domain Layer (Nội bộ):** Thực thể cốt lõi (`Stock.cs`), logic trừ kho; CẤM dính ORM, cấm tham chiếu module khác.
* **Application Layer (Nội bộ):** Use cases điều phối (`InProcessInventoryService.cs`).
* **Infrastructure Layer (Nội bộ):** `InventoryDbContext.cs` ánh xạ bảng CSDL; **khai báo internal**.
* **Contracts Layer (CÔNG KHAI):** Interface `IInventoryService.cs` và các DTOs nguyên thủy.

</div>
<div>

### 🏢 Ví Von: Quầy Giao Dịch Của Phòng
* **Contracts = Quầy giao dịch bên ngoài:** Người ngoài (Order Module) chỉ được đứng ở quầy nộp phiếu yêu cầu (`ReserveStockRequest`) và nhận kết quả.
* **Tủ hồ sơ nội bộ (DbContext):** Nằm sâu bên trong phòng, được khóa kín bằng từ khóa `internal`, người ngoài tuyệt đối không được tự ý bước vào lục lọi!

```csharp
// Kiểm soát truy cập bằng ngôn ngữ C#:
internal class InventoryDbContext : DbContext { ... }
public interface IInventoryService { ... } // Public
```

</div>
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Bên trong từng module, chúng ta tổ chức mã nguồn như thế nào để giữ được sự ngăn nắp?
Chúng ta kế thừa trọn vẹn mô hình Clean Architecture từ Buổi 4: Mỗi module có đủ tầng Domain, Application, Infrastructure và Contracts.
Hình tượng hóa đời thường:
Tầng Contracts chính là Quầy giao dịch của phòng Kho. Phòng Đơn hàng chỉ được phép đứng ở quầy đưa phiếu yêu cầu và nhận kết quả.
Còn tầng Infrastructure chứa DbContext chính là Chiếc tủ hồ sơ nội bộ của phòng. Chúng ta dùng từ khóa internal của C# để khóa chặt tủ này lại. Lập trình viên ở module Đơn hàng dù có cố tình cũng không thể inject DbContext của Kho vào code của mình!
-->

---

<!-- SLIDE 16: BỐN KỶ LUẬT CÔ LẬP DỮ LIỆU THÉP -->
# Bốn Kỷ Luật Cô Lập Dữ Liệu "Thép" Để Tránh Thoái Hóa Thành Monolith Rối ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Lỗi phổ biến nhất: Module giao tiếp chéo trực tiếp vào CSDL của nhau

<div class="grid-2">
<div>

### 🛡️ 4 Điều Luật Giới Nghiêm
1. **Schema riêng biệt:** Mỗi module 1 schema riêng (`order`, `inventory`, `notification`) trên PostgreSQL.
2. **CẤM TUYỆT ĐỐI SQL JOIN xuyên Schema:** Không bao giờ query nối bảng module khác.
3. **CẤM truy cập chéo DbContext:** Không inject DbContext của nhau.
4. **CẤM tạo Foreign Key cứng ở CSDL:** Chỉ lưu khóa logic (UUID).

</div>
<div>

### ⚠️ Lỗi Phổ Biến Nhất Cần Tránh
* **Modular trên danh nghĩa:** Đặt tên folder là module nhưng tiện tay viết câu SQL JOIN hoặc gọi DbContext chéo 
 →  Hệ thống vẫn là một Monolith rối rắm, sau này **không thể bóc tách ra Microservice**!
* **Phân biệt 2 cấp độ ranh giới:**
  * *Cấp mã nguồn:* Khóa bằng từ khóa `internal` và NetArchTest.
  * *Cấp CSDL:* Tách riêng schema PostgreSQL.

</div>
</div>

<div class="highlight">
🔗 <b>Chuyển giao vai trò:</b> Khi TV2 đã khóa chặt cánh cửa CSDL chung, làm thế nào để Đơn hàng và Kho giao tiếp với nhau? <b>Thành viên 3 sẽ làm sáng tỏ Huyết mạch Giao tiếp!</b>
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Để kết thúc phần của TV2, nhóm xin nhấn mạnh 4 kỷ luật cô lập dữ liệu thép:
1. Mỗi module một schema riêng trên PostgreSQL.
2. Tuyệt đối cấm SQL JOIN chéo bảng giữa 2 schema.
3. Tuyệt đối cấm dùng chung DbContext.
4. Tuyệt đối cấm tạo khóa ngoại cứng vật lý giữa các bảng của module khác nhau.
Tài liệu môn học đã cảnh báo: Lỗi phổ biến nhất là "Modular Monolith trên danh nghĩa"—chia folder rất đẹp nhưng tiện tay viết câu SQL JOIN. Một khi đã dính SQL JOIN, hệ thống sẽ vĩnh viễn không thể tách ra microservice được nữa.
Và khi cánh cửa database chung đã bị TV2 đóng chặt hoàn toàn, làm sao các module nói chuyện với nhau? Xin mời bạn Thành viên 3 trình bày về Huyết mạch giao tiếp!
-->

---

<!-- SLIDE 17: GIAO TIẾP ĐỒNG BỘ -->
# Giao Tiếp Đồng Bộ: Gọi Điện Hỏi Kho "Còn Hàng Không?" Và Đứng Máy Chờ <span class="tag-theory">[LÝ THUYẾT]</span>
### Thành viên 3 (Mục 2.4–2.6) — Cơ chế "Request - Response" và sự ràng buộc thời gian

* **Bản chất của Giao tiếp Đồng bộ (Synchronous Communication):** Tiến trình gọi (Caller) gửi yêu cầu và **tạm dừng luồng logic để chờ phản hồi ngay lập tức** rồi mới tiếp tục xử lý bước tiếp theo.
* **Tình huống bắt buộc trong câu chuyện:** Khi khách bấm mua chiếc laptop cuối cùng, phòng Đơn hàng bắt buộc phải gọi điện sang Kho hỏi: *"Còn chiếc laptop này không? Hãy giữ lại ngay!"*

<div class="grid-2">
<div class="card-order">

### 📞 Lời Gọi Đồng Bộ (Sync)
* **Khách 
 →  Order:** POST `/api/orders` (Sync).
* **Order 
 →  Inventory:** `ReserveStock` (Sync).
* *Bắt buộc phải biết còn hàng mới cho chốt đơn!*

</div>
<div class="card-notif">

### ⏳ Ràng Buộc Thời Gian (Temporal Coupling)
* Cả bên gọi và bên nhận **bắt buộc phải đồng thời trực tuyến** tại cùng thời điểm.
* Nếu phòng Kho bị nghẽn mạng 2 giây, phòng Đơn hàng cũng bị tắc nghẽn đứng chờ theo!

</div>
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Kính chào Thầy và các bạn, tôi là Thành viên 3, phụ trách phần giao tiếp đồng bộ và bất đồng bộ (mục 2.4 đến 2.6).
Sau khi TV2 đã khóa chặt cánh cửa CSDL chung, câu hỏi đặt ra là: Làm sao các module nói chuyện với nhau?
Đầu tiên là Giao tiếp đồng bộ. Bản chất của nó như một cuộc gọi điện thoại: Bạn nhấc máy gọi sang Kho hỏi "Còn chiếc laptop cuối cùng không?" và phải đứng máy chờ Kho trả lời.
Trong quy trình bán hàng, bước này bắt buộc phải đồng bộ: nếu không biết chắc còn hàng mà đã nhận tiền của khách thì cửa hàng sẽ vỡ trận.
Tuy nhiên, gọi đồng bộ tạo ra một ràng buộc thời gian nghẹt thở: cả hai bên phải cùng sống và cùng rảnh rỗi tại cùng một thời điểm!
-->

---

<!-- SLIDE 18: SO SÁNH REST VS GRPC -->
# Công Nghệ Đồng Bộ: RESTful API Dễ Dùng Cho Web, gRPC Tối Ưu Cho Mạng Nội Bộ <span class="tag-theory">[LÝ THUYẾT]</span>
### Hai vũ khí đồng bộ chủ đạo — Minh bạch công nghệ đã triển khai trong demo

| Tiêu chí kỹ thuật | RESTful API (HTTP/1.1 + JSON) | gRPC (HTTP/2 + Protocol Buffers) |
|---|---|---|
| **Định dạng dữ liệu** | Văn bản JSON dễ đọc bằng mắt, dễ debug Postman | Nhị phân nén nhỏ gọn, hợp đồng `.proto` nghiêm ngặt |
| **Giao thức truyền tải**| HTTP/1.1 tuần tự (mỗi request mở kết nối) | HTTP/2 Multiplexing (Dồn kênh trên 1 kết nối TCP) |
| **Kỷ luật hợp đồng** | Tùy chọn (Swagger / OpenAPI) | Bắt buộc compile-time qua file `.proto` |
| **Vị trí tối ưu** | **Public API Gateway** cho Web Browser / Mobile | **Internal Service-to-Service** trong mạng backend |
| **Thực tế trong bài** | **ĐÃ TRIỂN KHAI THẬT TRONG DEMO** (REST loopback)| **Lựa chọn tối ưu khi bóc tách Microservice độc lập** |

<div class="highlight">
🎯 <b>BANNER TRUNG THỰC (Quy tắc 7):</b> Demo hôm nay sử dụng <b>RESTful API qua HttpClient loopback</b> trong cùng tiến trình để chứng minh interface; gRPC là khuyến nghị kỹ thuật cao cấp khi bóc tách thành Microservice độc lập. Không ghi số đo độ trễ khi chưa kiểm thử thực tế.
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Khi cần gọi đồng bộ, chúng ta có 2 công nghệ lớn: RESTful API và gRPC.
RESTful API dùng HTTP và JSON, rất trực quan, đọc được bằng mắt thường, phù hợp làm cổng giao tiếp đối ngoại cho người dùng Web và App di động.
Trong khi đó, gRPC dùng HTTP/2 và nhị phân Protocol Buffers, tối ưu cho các service backend nói chuyện nội bộ với nhau.
Nhóm xin minh bạch tuyệt đối theo Quy tắc trung thực: Trong bài demo hôm nay, nhóm đã triển khai RESTful API thật qua HttpClient gọi loopback trong cùng tiến trình; còn gRPC là giải pháp kiến trúc đề xuất khi bóc tách hẳn thành microservice độc lập sau này.
-->

---

<!-- SLIDE 19: CASCADING FAILURE -->
# Rủi Ro Gọi Dây Chuyền (Cascading Failure) Và Nguy Cơ Cạn Kiệt Luồng Máy Chủ <span class="tag-theory">[LÝ THUYẾT]</span>
### Hiệu ứng domino: Một mắt xích bị chậm có thể kéo sập TOÀN BỘ hệ thống!

* **Hiệu ứng Domino trong chuỗi gọi đồng bộ:**
  Khách mua hàng 
 →  Order gọi Payment 
 →  Payment gọi tiếp sang Cổng ngân hàng bên ngoài. Nếu ngân hàng bị chập chờn mạng và treo 30 giây 
 →  Các luồng xử lý (Threads) tại Payment và Order đều bị chặn đứng (Blocked)!

```
[Khách hàng] ---> [Order Service] --------> [Payment Service] --------> [Cổng Ngân hàng]
                 (Thread: 200/200)         (Thread: 200/200)           (Mạng chập chờn,
                 [CẠN KIỆT THREAD]         [CẠN KIỆT THREAD]            Treo 30 giây!)
                         |
                         v
   HẬU QUẢ: Khách vào xem Catalog / Cart cũng bị LỖI 503 SERVICE UNAVAILABLE!
```

* **Ba (3) Cơ Chế Phòng Vệ Bắt Buộc:**
  1. **Timeout:** Bắt buộc giới hạn thời gian chờ tối đa (ví dụ 2 giây); quá giờ hủy ngay để giải phóng luồng.
  2. **Circuit Breaker (Ngắt mạch tự động):** Khi tỷ lệ lỗi vượt ngưỡng 
 →  ngắt mạch từ chối nhanh (Fail-Fast).
  3. **Bulkhead (Vách ngăn khoang tàu):** Cách ly hồ chứa luồng (Thread Pool) riêng cho từng dịch vụ.

<div class="warning-box">
🚪 <b>Lối thoát chiến lược:</b> Bước nào KHÔNG CẦN kết quả tức thì 
 →  Bắt buộc chuyển sang <b>Bất đồng bộ</b>!
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Điểm nguy hiểm nhất của giao tiếp đồng bộ chính là Cascading Failure – Lỗi dây chuyền.
Hãy hình dung hàng xe ô tô nối đuôi nhau: chiếc xe phía trước chết máy thì toàn bộ đoàn xe phía sau bị kẹt cứng.
Nếu Order gọi Payment, Payment gọi cổng ngân hàng mà ngân hàng bị treo mạng, các thread tại máy chủ của chúng ta sẽ bị giam giữ. Chỉ vài giây là toàn bộ Thread Pool cạn kiệt!
Hậu quả là khách khác vào xem trang chủ sản phẩm cũng bị báo lỗi 503 sập server!
Để sống sót, ta phải cài đặt Timeout, Circuit Breaker, và quan trọng nhất: chuyển các bước phụ trợ sang Message Queue bất đồng bộ.
-->

---

<!-- SLIDE 20: BẤT ĐỒNG BỘ VÀ MESSAGE QUEUE -->
# Bất Đồng Bộ Và Message Queue: Thả Phiếu Vào Hòm Thư Trung Gian Để Giảm Phụ Thuộc ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Phản hồi tức thì cho người dùng và điều hòa xung nhịp tải (Load Leveling)

* **Vì sao cần Bất đồng bộ (Asynchronous):** Không phải bước nào trong luồng mua hàng cũng cần bắt khách đứng chờ. Sau khi tạo đơn thành công: việc gửi email xác nhận (Notification) hay cập nhật thống kê doanh số đều có thể xử lý sau đó một chút!
* **Message Queue là gì:** Một hệ thống trung gian nhận thông điệp từ bên gửi (Producer/Publisher) và lưu trữ tạm thời, để bên nhận (Consumer/Subscriber) lấy ra xử lý theo nhịp độ riêng của mình.

<div class="grid-2">
<div class="card-neutral">

### 📬 Ví Von Hòm Thư Trung Gian
* Bên gửi thả thư vào hòm rồi đi làm việc khác, không cần đứng chờ người nhận mở thư.
* Bên gửi và bên nhận **không cần hoạt động cùng lúc, không cần biết sự tồn tại của nhau**.

</div>
<div class="card-neutral">

### ⚡ Hai Giá Trị Vàng Thực Tế
* **Phản hồi siêu nhanh (Loose Coupling):** Khách nhận kết quả đặt hàng ngay (< 50ms) thay vì chờ gửi email.
* **Điều hòa tải (Load Leveling):** Giờ Flash Sale, 10.000 đơn đổ về; worker cứ túc tắc rút thư xử lý mà không làm sập server!

</div>
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Để giải phóng hệ thống khỏi sự ràng buộc nghẹt thở của giao tiếp đồng bộ, chúng ta đến với Bất đồng bộ qua Message Queue.
Ví von đời thường: Message Queue chính là chiếc Hòm thư trung gian của tòa nhà.
Sau khi chốt đơn xong, phòng Đơn hàng chỉ việc viết một bức thư báo tin và thả vào hòm thư bưu điện rồi quay ra tiếp khách tiếp theo.
Phòng Thông báo khi nào rảnh tay thì mở hòm thư lấy phiếu ra gửi email. Hai phòng không cần nhìn mặt nhau, không cần biết nhau ở tầng mấy!
Nhờ đó, khách hàng nhận phản hồi thành công ngay lập tức, và vào giờ cao điểm, hòm thư sẽ đóng vai trò bộ đệm điều hòa xung nhịp tải vô cùng êm ái.
-->

---

<!-- SLIDE 21: RABBITMQ 6 KHÁI NIỆM & 3 EXCHANGE -->
# RabbitMQ: Bưu Điện Nội Bộ Với Sáu Khái Niệm Cốt Lõi Và Ba Loại Exchange ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Tiêu chuẩn công nghiệp AMQP 0-9-1 — Bàn phân loại thư và các hòm thư chuyên dụng

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
                  +-------------------------------------------------+
```

<div class="grid-2">
<div>

### 📮 Sáu (6) Khái Niệm Cốt Lõi
1. **Producer:** Bên tạo và gửi thông điệp.
2. **Exchange:** Bàn phân loại thư trung tâm.
3. **Binding:** Quy tắc thư này vào hòm nào.
4. **Queue:** Hòm thư chứa tin nhắn FIFO.
5. **Consumer:** Worker lấy thư ra xử lý.
6. **Routing Key:** Địa chỉ ghi trên bì thư.

</div>
<div>

### 🔀 Ba (3) Loại Exchange Chính
* **Direct Exchange:** Gửi đúng địa chỉ (khớp chính xác 100% Routing Key).
* **Topic Exchange:** Gửi theo mẫu địa chỉ (so khớp ký tự đại diện `*` và `#`).
* **Fanout Exchange:** **BỎ QUA ĐỊA CHỈ!** Photo một bức thư gửi vào TẤT CẢ các hòm thư đã đăng ký liên kết.

</div>
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Chuẩn công nghiệp phổ biến nhất cho Message Queue là RabbitMQ theo chuẩn AMQP 0-9-1.
RabbitMQ hoạt động y hệt một Bưu điện nội bộ với 6 khái niệm:
Producer là người gửi thư. Exchange là bàn phân loại thư trung tâm. Binding là bảng quy tắc phân loại. Queue là hòm thư. Consumer là nhân viên bưu tá nhận thư đi phát.
Và hãy nhớ nằm lòng nguyên tắc vàng: Producer không bao giờ gửi thẳng vào Queue! Producer chỉ gửi vào Exchange.
Exchange có 3 loại: Direct là chuyển đúng đích danh; Topic là chuyển theo mẫu; và Fanout là photo nhân bản gửi cho tất cả các hòm thư đã đăng ký!
-->

---

<!-- SLIDE 22: PUBLISH/SUBSCRIBE QUA FANOUT -->
# Publish/Subscribe Qua Fanout Exchange: Một Sự Kiện, Nhiều Phòng Ban Tự Xử Lý ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Sự kiện OrderCreated — Một lần phát tán, các phòng ban xử lý hoàn toàn song song

```
                     MÔ HÌNH PUB/SUB QUA FANOUT EXCHANGE
                          +-----------------------+
                          |    Fanout Exchange    |
                          |    ('order.events')   |
                          +-----------------------+
                                      |
                +---------------------+---------------------+
                |                                           |
                v                                           v
      +-------------------+                       +-------------------+
      | Queue: inventory  |                       |Queue: notification|
      +-------------------+                       +-------------------+
                |                                           |
                v                                           v
      [Inventory Consumer]                        [Notification Consumer]
      (Chuyển giữ -> bán)                         (Gửi Email mô phỏng)
```

* **Cơ chế phối hợp thực tế:** Order publish 1 lần sự kiện `OrderCreated` lên Exchange `order.events` 
 →  Exchange tự động nhân bản thông điệp thành 2 bản đẩy vào 2 queue độc lập 
 →  Kho và Thông báo tự rút thư về xử lý song song; phòng Đơn hàng không phải chờ đợi!
* *(Ghi chú minh bạch: Demo thực hiện 2 Consumer là Inventory và Notification; Promotion là ví dụ mở rộng trong đề).*

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Đây là cơ chế Publish/Subscribe qua Fanout Exchange trong bài toán bán hàng:
Khi khách mua xong chiếc laptop, phòng Đơn hàng chỉ cần gửi một bức thư duy nhất mang tên OrderCreated lên bàn phân loại Fanout Exchange.
Exchange tự động photo nhân bản thành 2 bức thư giống hệt nhau và thả vào 2 hòm thư riêng: một hòm cho phòng Kho, và một hòm cho phòng Thông báo.
Phòng Kho lấy thư ra để cập nhật sổ sách từ "đang giữ" sang "đã bán".
Phòng Thông báo lấy thư ra để gửi email cho khách.
Hai phòng ban chạy hoàn toàn song song và độc lập. Đó là lý do hệ thống vận hành cực kỳ trơn tru!
-->

---

<!-- SLIDE 23: BẢN ĐỒ GIAO TIẾP VÀ LUỒNG CHIẾC LAPTOP CUỐI -->
# Bản Đồ Giao Tiếp Của Luồng Mua Chiếc Laptop Cuối Cùng ★ <span class="tag-theory">[LÝ THUYẾT]</span>
### Kết hợp nhịp nhàng: Cần kết quả ngay thì Đồng bộ, chỉ báo sự kiện thì Bất đồng bộ

```
[Khách hàng]
     |
     | (1) POST /api/orders (Đồng bộ)
     v
[Phòng ĐƠN HÀNG] ──(2) ReserveStock (Đồng bộ: mũi tên liền)──> [Phòng KHO]
     |                                                              | (Kho giữ hàng:
     | (3) Lưu đơn Confirmed vào CSDL order                         |  available=0,
     |                                                              |  reserved=1)
     | - - (4) Publish 'OrderCreatedEvent' (Bất đồng bộ: đứt) - - > [RABBITMQ BROKER]
     |                                                                   |
     | (5) Trả lời khách: HTTP 201 Created (< 50ms)                      +---------+
     v                                                                   |         |
Khách nhận thông báo thành công tức thì!                                 v         v
                                                           [Queue: Kho]  [Queue: Notif]
                                                                |             |
                                                                v             v
                                                           (Chuyển bán)  (Gửi Email)
```

* **Bộ quy tắc quyết định:** Cần kết quả ngay để quyết định bước tiếp (Kiểm tra & Giữ kho) 
 →  Dùng Đồng bộ (mũi tên liền `──>`). Chỉ cần báo một việc đã xảy ra (Gửi mail, trừ kho ngầm) 
 →  Dùng Bất đồng bộ (mũi tên đứt `- - >`).

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Slide này là bức tranh toàn cảnh về bản đồ giao tiếp của luồng mua chiếc laptop cuối cùng:
Bước 1: Khách nhấn mua.
Bước 2: Phòng Đơn hàng gọi đồng bộ (mũi tên liền màu xanh) sang phòng Kho để giữ hàng. Bước này bắt buộc phải đồng bộ vì phải chắc chắn giữ được hàng thì mới cho đặt!
Bước 3: Đơn hàng lưu vào CSDL của mình.
Bước 4: Đơn hàng thả sự kiện OrderCreated vào hòm thư RabbitMQ (mũi tên nét đứt).
Bước 5: Trả lời ngay cho khách mã 201 Created. Khách nhận thông báo thành công siêu tốc.
Còn việc trừ kho chính thức và gửi email diễn ra bất đồng bộ phía sau. Đây chính là bản giao hưởng kết hợp hoàn hảo giữa Sync và Async!
-->

---

<!-- SLIDE 24: ĐỘ TIN CẬY CỦA RABBITMQ -->
# Tính Tin Cậy Của Message Queue Trong Hệ Thống Phân Tán <span class="tag-theory">[LÝ THUYẾT]</span>
### Bất đồng bộ không đồng nghĩa với mất tin — Các cơ chế bảo vệ thông điệp

* **Các cơ chế kỹ thuật bảo đảm an toàn của RabbitMQ:**
  1. **Queue Durable (`durable = true`):** Metadata hàng đợi lưu trên đĩa, sống sót qua các lần khởi động lại broker.
  2. **Message Persistent (`delivery_mode = 2`):** Thông điệp được ghi vào Disk Log trước khi xác nhận.
  3. **Publisher Confirms:** Bên gửi chỉ coi là thành công khi nhận được tín hiệu ACK từ RabbitMQ Broker.
  4. **Consumer Manual ACK (`autoAck = false`):** Xử lý nghiệp vụ & commit CSDL xong mới gửi lệnh `basicAck`.

<div class="grid-2" style="margin-top: 10px;">
<div class="card-inv">

### ✔ Demo Đã Chứng Minh Bằng Thực Tế
* Tắt Consumer Notification 
 →  Message nằm chờ trong Queue (`Ready = 1`).
* Bật lại Consumer 
 →  Kéo tin về xử lý, ghi DB và gửi ACK 
 →  Queue trở về `Ready = 0`.

</div>
<div class="card-neutral">

### ⚠️ Giới Hạn Cần Minh Bạch
* Chưa triển khai **Transactional Outbox**: Nếu server sập ngay sau khi commit DB mà chưa kịp publish vào RabbitMQ thì sự kiện vẫn có thể bị rớt. Không dùng chữ "Zero Message Loss tuyệt đối"!

</div>
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Nhiều người lầm tưởng dùng Message Queue thì dữ liệu dễ bị mất.
Thực ra, RabbitMQ bảo vệ tin nhắn rất kiên cố bằng hàng đợi Durable, tin nhắn Persistent lưu đĩa và cơ chế Manual ACK – worker xử lý xong mới gửi lệnh xóa tin.
Nhóm xin minh bạch tuyệt đối: Trong demo hôm nay, nhóm đã chứng minh việc tắt Consumer thì tin nhắn vẫn nằm chờ trong queue và xử lý lại khi bật lên.
Tuy nhiên, để đạt độ an toàn cao nhất trong doanh nghiệp, hệ thống cần bổ sung Transactional Outbox Pattern để phòng ngừa trường hợp máy chủ sập ngay sau khi ghi CSDL mà chưa kịp publish ra broker.
Và bây giờ, kính mời Thầy và các bạn cùng chứng kiến toàn bộ 4 phần Demo thực tế ngay sau đây!
-->

---

<!-- SLIDE 25: DEMO 1 - CẤU TRÚC MODULAR MONOLITH -->
# DEMO 1: Cấu Trúc Modular Monolith Và Ba Schema CSDL Độc Lập <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Thành viên 2 — Kiểm chứng ranh giới mã nguồn và sự cô lập CSDL trên PostgreSQL 16

<div class="grid-2">
<div class="card-neutral">

### 📁 1. Ranh Giới Solution & Project
* `Order`: Quản lý đơn hàng (chỉ tham chiếu Contracts).
* `Inventory`: Quản lý kho hàng & logic giữ hàng.
* `Inventory.Contracts`: Public Interface `IInventoryService`.
* `Notification`: Gửi email/SMS mô phỏng.
* `Shared.Kernel`: RabbitMQ Connection & Event Contract.

</div>
<div class="card-neutral">

### 🗄️ 2. Ba Schema PostgreSQL Riêng Biệt
```sql
SELECT schema_name FROM information_schema.schemata 
WHERE schema_name IN ('order', 'inventory', 'notification');
-- Kết quả trả về thực tế: Đủ 3 schema độc lập!
```
* **Kỷ luật CSDL:** 100% `DbContext` khai báo `internal`.
* **Kiểm thử kiến trúc:** NetArchTest kiểm chứng Order không tham chiếu nội bộ Inventory 
 →  **PASS!**

</div>
</div>

<div class="highlight">
✔ <b>Thực chứng:</b> Cấu trúc "Tòa nhà một cổng, chia 3 phòng có chìa khóa riêng" đã hoạt động ổn định trên CSDL thật!
</div>

<!-- Footer: TV2 trình bày -->

<!-- Speaker Notes:
Bắt đầu phần Demo thực tế:
Tôi xin trình diễn Demo phần 1: Kiểm chứng cấu trúc Modular Monolith và cô lập dữ liệu.
Trên màn hình VS Code, các bạn thấy rõ 3 module Order, Inventory, Notification được tách thành các project riêng biệt.
Module Order tuyệt đối không tham chiếu vào project Inventory nội bộ, mà chỉ nhìn thấy project nhẹ nhàng mang tên Inventory.Contracts chứa interface IInventoryService.
Ở dưới CSDL PostgreSQL 16, chúng tôi thực thi truy vấn và màn hình trả về đúng 3 schema riêng biệt: order, inventory, notification.
Bộ kiểm thử kiến trúc NetArchTest tự động quét assembly và xác nhận: 100% DbContext là internal, không có bất kỳ hành vi gọi chéo CSDL nào.
-->

---

<!-- SLIDE 26: DEMO 2 - GIAO TIẾP ĐỒNG BỘ -->
# DEMO 2: Giao Tiếp Đồng Bộ Kiểm Tra Kho — Đủ Hàng (201) vs Hết Hàng (409) <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Thành viên 1 — Kiểm chứng quy trình: Gọi kiểm tra 
 →  Giữ hàng nguyên tử 
 →  Đổi mode HTTP

* **Thao tác 1 (Đủ hàng):** Khách đặt 2 chiếc Laptop Gaming Pro 16 (kho có 5 chiếc).
  * Lời gọi đồng bộ `IInventoryService.ReserveAsync` 
 →  Tạm giữ 2 chiếc thành công.
  * Phản hồi máy chủ: **HTTP 201 Created** | Trạng thái `Confirmed` | Tổng tiền 50.000.000 VNĐ.
* **Thao tác 2 (Hết hàng):** Khách đặt 1 chiếc Chuột Không Dây RGB (kho tồn = 0 chiếc).
  * Lời gọi đồng bộ `IInventoryService.ReserveAsync` 
 →  Báo kho không đủ hàng.
  * Phản hồi máy chủ: **HTTP 409 Conflict** | Trạng thái `Rejected` | Thông báo: *"Tồn kho khả dụng không đủ"*.
* **Thao tác 3 (Chuyển mode HTTP REST):**
  * Đổi cấu hình `Inventory:Mode = Http` trong `appsettings.json`.
  * Thực thi đặt hàng 
 →  Kết quả HTTP 201 Created hoàn toàn tương tự qua lời gọi REST loopback!

<div class="highlight">
🎯 <b>Điểm then chốt:</b> Lớp <code>OrderService.cs</code> <b>không thay đổi dù chỉ một dòng code</b> khi chuyển đổi giữa chế độ gọi trực tiếp trong bộ nhớ (InProcess) và gọi qua REST API mạng!
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Tôi là TV1, xin trình diễn Demo phần 2: Giao tiếp đồng bộ.
Đầu tiên, tôi gửi request đặt mua 2 chiếc Laptop Gaming Pro. Order gọi đồng bộ sang Kho. Kho còn 5 chiếc nên tạm giữ 2 chiếc, trả về thành công và Order phản hồi ngay HTTP 201 Created.
Tiếp theo, tôi gửi request mua 1 chiếc Chuột RGB. Sản phẩm này tồn kho bằng 0. Kho từ chối giữ hàng, Order lập tức phản hồi HTTP 409 Conflict và lưu đơn hàng ở trạng thái Rejected.
Và điều tuyệt vời nhất: Khi tôi vào cấu hình đổi Inventory:Mode sang Http, Order chuyển sang gọi REST endpoint qua loopback. Kết quả đơn hàng vẫn tạo thành công y hệt mà code của OrderService không hề bị sửa đổi!
-->

---

<!-- SLIDE 27: DEMO 3 - RABBITMQ THẬT & CONSUMER OFFLINE -->
# DEMO 3: RabbitMQ Thật — Tắt Consumer, Tin Nhắn Vẫn Nằm Chờ An Toàn ★ <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Thành viên 3 — Kiểm chứng tính bất đồng bộ và sự bền vững của hàng đợi trên Broker thật

<div class="grid-2">
<div>

### 🛑 Thực Nghiệm Consumer Offline
* **Bước 1:** Gọi `POST /demo/consumers/notification/stop` 
 →  Tắt Consumer Notification (Status: `OFFLINE`).
* **Bước 2:** Khách đặt mua thêm 1 Laptop 
 →  Server chốt đơn và trả về **HTTP 201 Created** ngay tức thì!
* **Bước 3:** Mở RabbitMQ Management UI (`:15672`): Hàng đợi `notification.order-created` lưu tin nhắn an toàn trên đĩa cứng (**Ready = 1**)!

</div>
<div>

### 🚀 Hồi Phục Consumer Online
* **Bước 4:** Gọi `POST /demo/consumers/notification/start` 
 →  Khởi động lại Consumer Notification.
* **Kết quả ngay lập tức:**
  * Queue Depth lập tức trở về **Ready = 0**!
  * Consumer kéo tin về, ghi nhận email mô phỏng vào CSDL `notification.notification_logs` và gửi Manual ACK (`basicAck`)!

</div>
</div>

<div class="highlight">
✔ <b>Thực chứng:</b> Dù Consumer có tạm ngừng hoạt động, thông điệp vẫn nằm chờ an toàn trong hàng đợi RabbitMQ, không hề có tình trạng mất mát thông tin!
</div>

<!-- Footer: TV3 trình bày -->

<!-- Speaker Notes:
Tôi là TV3, xin trình diễn Demo phần 3: Độ tin cậy của RabbitMQ thật.
Nhiều người e ngại bất đồng bộ dễ mất tin. Tôi sẽ chứng minh điều ngược lại trên RabbitMQ Management UI thật tại cổng 15672:
Đầu tiên, tôi gọi API tạm dừng Consumer Notification. Trạng thái consumer chuyển sang OFFLINE.
Lúc này, tôi tiến hành đặt một đơn hàng mới. Đơn hàng vẫn hoàn tất thành công với mã 201 Created.
Trên giao diện RabbitMQ UI, Thầy và các bạn thấy rõ: Hàng đợi notification đang chứa đúng 1 message ở trạng thái Ready!
Bây giờ, tôi bấm API bật lại Consumer. Ngay trong tích tắc, tin nhắn được kéo về xử lý, log email mô phỏng xuất hiện trong database và hàng đợi trở về số 0 tròn trĩnh!
-->

---

<!-- SLIDE 28: DEMO 4 - RACE CONDITION & BỘ 5 TEST -->
# DEMO 4: Tranh Chấp Chiếc Laptop Cuối Cùng (`race.ps1`) Và Bộ 5 Test Tự Động <span class="tag-demo">[DEMO BẮT BUỘC]</span>
### Thành viên 2 & 3 — Giải quyết bài toán mở đầu: Chống bán vượt tồn bằng SQL Atomic Update

<div class="grid-2">
<div>

### 🏎️ Kịch Bản Chạy Script `demo/race.ps1`
* Tồn kho khả dụng của Laptop Cuối: **đúng 1 chiếc**.
* Bắn đồng thời 2 HTTP requests song song:
  * **Khách hàng 1:** Nhận **HTTP 201 Created** (Chốt đơn).
  * **Khách hàng 2:** Nhận **HTTP 409 Conflict** (Hết hàng).
* **Kết quả kho:** Khả dụng = `0`, Đã bán = `1`. Không bao giờ bị bán âm kho!
* *Ba khái niệm:* **Kiểm tra kho** 
 →  **Giữ hàng (Reserve)** 
 →  **Trừ hàng chính thức (Sold)** khi có ACK.

</div>
<div>

### 🧪 Bộ 5 Tests Tự Động xUnit (`ModularMonolith.Tests`)
```text
Passed! - Failed: 0, Passed: 5, Skipped: 0
Duration: 4 seconds
```
* `Test1`: Đủ hàng 
 →  201 Created (Confirmed).
* `Test2`: Hết hàng 
 →  409 Conflict (Rejected).
* `Test3`: 2 request đồng thời 
 →  Đúng 1x201 và 1x409.
* `Test4`: NetArchTest kiểm chứng Order độc lập.
* `Test5`: NetArchTest kiểm chứng DbContext internal.

</div>
</div>

<!-- Footer: TV2 & TV3 trình bày -->

<!-- Speaker Notes:
Ở đầu bài thuyết trình, chúng tôi đã đặt ra câu hỏi hóc búa: Nếu 2 khách cùng tranh mua chiếc laptop cuối cùng thì sao?
Đây là câu trả lời của nhóm trong Demo 4:
Chúng tôi chạy script PowerShell race.ps1 bắn đồng thời 2 request song song.
Nhờ câu lệnh SQL UPDATE nguyên tử có điều kiện available >= qty, PostgreSQL khóa dòng và xử lý tuần tự: Khách hàng 1 may mắn nhận mã 201 Created; Khách hàng 2 nhận ngay mã 409 Conflict báo hết hàng. Kho trở về 0, tuyệt đối không bị bán âm!
Toàn bộ 5 bài test tự động xUnit cũng đã chạy thực tế trên máy và vượt qua 100% trong 4 giây.
-->

---

<!-- SLIDE 29: NẾU TÁCH INVENTORY THÀNH MICROSERVICE -->
# Con Đường Tiến Hóa: Nếu Sau Này Cần Tách Inventory Thành Microservice Độc Lập <span class="tag-ext">[HƯỚNG MỞ RỘNG]</span>
### Thành viên 1 — Phân tích chi tiết: Đổi những gì và Giữ nguyên những gì?

| Hạng mục kỹ thuật | Giai đoạn hiện tại (Modular Monolith) | Khi tách thành Microservice độc lập |
|---|---|---|
| **Mã nguồn nghiệp vụ (Core Logic)**| Nằm trong `Inventory.Domain` | **GIỮ NGUYÊN 100% (Không viết lại logic!)** |
| **Giao kèo dịch vụ (Contract)** | Nằm trong `Inventory.Contracts` | **GIỮ NGUYÊN 100% (Interface `IInventoryService`)** |
| **Mã nguồn phòng Đơn hàng** | `OrderService.cs` gọi qua Interface | **GIỮ NGUYÊN 100% (Không sửa 1 dòng code!)** |
| **Cơ sở dữ liệu (Database)** | Schema `inventory` chung 1 server PG | **TÁCH THÀNH CSDL VẬT LÝ RIÊNG** |
| **Cách thức giao tiếp (Network)** | Gọi in-process trong bộ nhớ RAM | **Dùng `HttpInventoryService` trỏ sang URL mới** |
| **Đóng gói & CI/CD Pipeline** | 1 Container chung, 1 Pipeline chung | **Thêm Dockerfile riêng & Pipeline CI/CD riêng** |

<div class="warning-box">
⚠️ <b>Cảnh báo thực tế khi bóc tách:</b> Phải giải quyết thêm các bài toán mới: Độ trễ mạng (Network Latency), Quản lý token bảo mật liên service, Giám sát tập trung (Distributed Tracing), và Xử lý lỗi phân tán.
</div>

<!-- Footer: TV1 trình bày -->

<!-- Speaker Notes:
Nếu sau này công ty phát triển vượt bậc và đợt sale 11.11 đòi hỏi phải tách phòng Kho thành Microservice độc lập thì sao?
Bảng so sánh này chỉ rõ:
Những gì GIỮ NGUYÊN 100%: Toàn bộ code nghiệp vụ trừ kho của Inventory.Domain, interface IInventoryService, và đặc biệt là toàn bộ code của OrderService.
Những gì CẦN THAY ĐỔI: Chuyển schema inventory sang server database riêng, đổi cấu hình HttpClient trong Order trỏ tới địa chỉ IP của service mới, đóng gói Dockerfile riêng và thiết lập pipeline CI/CD riêng.
Đó chính là giá trị vô giá của tư duy Modular Monolith ngay từ ngày đầu!
-->

---

<!-- SLIDE 30: TỔNG KẾT VÀ GIỚI HẠN DEMO -->
# Tổng Kết Thế "Kiềng 3 Chân" Và Các Giới Hạn Kỹ Thuật Trung Thực Của Bản Demo <span class="tag-theory">[TỔNG KẾT]</span>
### Sự gắn kết hữu cơ không thể tách rời giữa 3 mảng nghiên cứu & Minh bạch giới hạn

```
                    TRỤ CỘT 1: KIẾN TRÚC TỔNG THỂ (TV1)
                     * Chiến lược MonolithFirst & Kiến trúc lai
                     * Lộ trình tiến hóa 4 bước chuẩn mực
                                    | (Định hình tư duy phân rã)
                                    v
                    TRỤ CỘT 2: RANH GIỚI NGHIỆP VỤ (TV2)
                     * Bounded Context, Bộ 4 câu hỏi vàng
                     * 4 Kỷ luật cô lập CSDL: CẤM SQL JOIN CHÉO
                                    | (Cấm JOIN chéo -> Cần giao tiếp mới)
                                    v
                    TRỤ CỘT 3: CƠ CHẾ GIAO TIẾP (TV3)
                     * Đồng bộ (Sync): Gọi kiểm tra kho trước khi chốt đơn
                     * Bất đồng bộ (RabbitMQ): Thả sự kiện vào hòm thư chạy ngầm
```

<div class="warning-box">
🔍 <b>BẢNG GIỚI HẠN KỸ THUẬT TRUNG THỰC (Quy tắc 7):</b>
<br>1. <b>Gửi Email/SMS:</b> Là mô phỏng (Simulation) ghi log và CSDL, chưa kết nối SMTP thật.
<br>2. <b>Giao tiếp REST:</b> Chế độ Http hiện tại gọi loopback cùng process, chưa tách máy chủ riêng.
<br>3. <b>CSDL:</b> Dùng 1 PostgreSQL server chia 3 schema, chưa phân quyền user DB riêng biệt.
<br>4. <b>Transactional Outbox:</b> Chưa triển khai outbox table, publish trực tiếp sau khi lưu đơn.
</div>

<!-- Footer: Toàn nhóm trình bày -->

<!-- Speaker Notes:
Để khép lại bài thuyết trình, nhóm xin đúc kết thế "Kiềng 3 chân" hữu cơ:
TV1 định hướng chiến lược kiến trúc Modular Monolith.
TV2 vạch ra Bounded Context và 4 kỷ luật cô lập dữ liệu thép để CSDL không bị rối.
TV3 thiết lập huyết mạch giao tiếp: Đồng bộ để giữ hàng và Bất đồng bộ để thông báo sự kiện.
Đồng thời, với tinh thần kỹ sư trung thực, nhóm công khai 4 giới hạn hiện tại của bản demo: email là mô phỏng, REST gọi loopback, CSDL chia theo schema, và chưa triển khai Transactional Outbox.
Nhóm xin chân thành cảm ơn Thầy và các bạn đã chú ý lắng nghe!
-->

---

<!-- SLIDE 31: PHỤ LỤC A - MA TRẬN 10 MODULE ĐẦY ĐỦ -->
# PHỤ LỤC A: Ma Trận Toàn Diện Mười Nghiệp Vụ E-Commerce × Bộ 4 Câu Hỏi Vàng <span class="tag-app">[PHỤ LỤC]</span>
### Tài liệu đối chiếu chuyên sâu phục vụ phiên chất vấn và phản biện của Hội đồng

| STT | Nghiệp vụ (Module) | 1. Ngôn ngữ riêng | 2. Phòng ban thực tế | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Đề xuất kiến trúc |
|:---:|---|---|---|---|:---:|:---:|
| **1** | **Catalog** (Sản phẩm) | Attributes, Specs, Giá bán | Merchandising | Độc lập cao: Lưu thông tin mô tả, ảnh | Thấp - TB | **TÁCH RIÊNG (Lõi)** |
| **2** | **Cart** (Giỏ hàng tạm) | ShoppingCart, CartItem | Growth & UX | Độc lập cao: Lưu Redis/Schema riêng, xóa sau 7 ngày | Trung bình | **TÁCH RIÊNG (Lõi)** |
| **3** | **Order** (Đơn hàng lõi) | SalesOrder, Snapshot | Vận hành Đơn & Kế toán | Độc lập cao: Lưu **Snapshot** sản phẩm; cấm JOIN | Thấp - TB | **TÁCH LÕI** |
| **4** | **Payment** (Thanh toán) | Giao dịch, Đối soát | Tài chính - Kế toán | Tuyệt đối: Lưu log đối soát, liên kết qua OrderId | Thấp (PCI-DSS)| **TÁCH RIÊNG (Lõi)** |
| **5** | **Inventory** (Tồn kho) | SKU, Vị trí kệ, ATP | Quản lý Kho | Độc lập cao: Quản lý số lượng tồn theo SKU | Thấp | **TÁCH RIÊNG (Lõi)** |
| **6** | **Shipping** (Giao vận) | Waybill, Consignment | Logistics & 3PL | Độc lập cao: Lưu địa chỉ giao, cân nặng, mã tracking | Trung bình | **TÁCH RIÊNG (Lõi)** |
| **7** | **Identity** (Định danh) | UserAccount, Role, JWT | An ninh mạng & IT | Tuyệt đối: Lưu tài khoản, mật khẩu; module khác lưu UserId | Rất thấp | **TÁCH RIÊNG (Lõi)** |
| **8** | **Notification** (Thông báo)| Template, Kênh Email/SMS | Bộ phận CSKH | Tuyệt đối: Nhận payload từ Queue; không lưu data | Trung bình | **TÁCH RIÊNG (Lõi)** |
| **9** | **Promotion** (Khuyến mãi)| Voucher, Quota, FlashSale | Tiếp thị (Marketing) | Độc lập cao: Tính tiền giảm dựa trên DTO đầu vào | **RẤT CAO** | **TÁCH RIÊNG (Mở rộng)**|
| **10**| **Review** (Đánh giá) | Rating 1-5 sao, Badge | Quản trị Chất lượng | Độc lập cao: Lưu đánh giá theo ProductId và UserId | Thấp | **TÁCH RIÊNG (Mở rộng)**|

<!-- Footer: Phụ lục tra cứu -->

<!-- Speaker Notes:
Đây là Phụ lục A cung cấp bảng ma trận đầy đủ cho cả 10 nghiệp vụ Thương mại điện tử được đánh giá chi tiết theo Bộ 4 câu hỏi vàng.
Slide này dùng để đối chiếu khi Giảng viên hỏi sâu về lý do phân rã của từng module cụ thể như Shipping, Identity, Promotion hay Review.
-->

---

<!-- SLIDE 32: PHỤ LỤC B - BẢNG THUẬT NGỮ VÀ VÍ VON -->
# PHỤ LỤC B: Bảng Thuật Ngữ Kỹ Thuật Và Ví Von Đời Thường Chuẩn Xác <span class="tag-app">[PHỤ LỤC]</span>
### Hệ thống hóa ngôn ngữ giao tiếp giúp người ngoài ngành dễ dàng thấu hiểu kiến trúc

| Thuật ngữ kỹ thuật | Diễn giải đời thường | Bản chất kỹ thuật trong hệ thống phần mềm |
|---|---|---|
| **Monolithic** | Quán nhỏ, một người làm hết mọi việc | Một codebase, một process, dùng chung một database duy nhất |
| **Modular Monolithic** | Tòa nhà một cổng, chia 3 phòng có khóa riêng | Một codebase, một process, chia module nghiêm ngặt, schema CSDL riêng |
| **Microservices** | Chuỗi chi nhánh, mỗi nơi một tòa nhà ngoài phố | Đa process phân tán trên mạng, database riêng cho từng dịch vụ |
| **Bounded Context** | Mỗi phòng hiểu từ "sản phẩm" theo nghĩa riêng | Ngữ cảnh nghiệp vụ độc lập, ranh giới phòng vệ mô hình dữ liệu (DDD) |
| **Interface công khai** | Quầy giao dịch của mỗi phòng ban | Hợp đồng dịch vụ trừu tượng (`IInventoryService`) làm đường cắt kiến trúc |
| **DbContext / CSDL** | Tủ hồ sơ nội bộ của phòng, không cho người ngoài tự mở | Lớp truy xuất CSDL nội bộ được bảo vệ bằng từ khóa `internal` |
| **Đồng bộ (REST/gRPC)** | Gọi điện thoại hỏi kho và đứng máy chờ trả lời | Caller gửi request và chặn luồng logic đợi response tức thời |
| **Bất đồng bộ (Async)** | Thả phiếu vào hòm thư, các phòng tự lấy xử lý | Caller phát sự kiện ra Message Queue rồi giải phóng luồng ngay |
| **RabbitMQ** | Bưu điện nội bộ của hệ thống | Hệ thống Message Broker trung gian theo chuẩn AMQP 0-9-1 |
| **Fanout Exchange** | Photo một bức thư gửi vào mọi hòm đã đăng ký | Bộ phân loại nhân bản và phát tán thông điệp 1-nhiều (Publish/Subscribe) |
| **Cascading Failure** | Hàng domino đổ, xe nối đuôi chết máy | Lỗi sụp đổ dây chuyền và cạn kiệt Thread Pool do gọi đồng bộ nhiều tầng |

<!-- Footer: Phụ lục tra cứu -->

<!-- Speaker Notes:
Phụ lục B là bảng đối chiếu thuật ngữ kỹ thuật và ví von đời thường chuẩn xác theo đúng Bảng B của kịch bản.
Bảng này giúp nhóm luôn giữ được sự nhất quán tuyệt đối trong lời nói khi thuyết trình và trả lời chất vấn của hội đồng.
-->

---

<!-- SLIDE 33: PHỤ LỤC C - MA TRẬN 8 LUỒNG SYNC/ASYNC & NGUỒN -->
# PHỤ LỤC C: Ma Trận 8 Luồng Nghiệp Vụ Thực Tế & Tài Liệu Tham Khảo <span class="tag-app">[PHỤ LỤC]</span>
### Phân định rõ ràng cơ chế giao tiếp và danh mục tài nguyên học thuật chính thống

| # | Luồng nghiệp vụ cụ thể | Cơ chế chọn | Công nghệ khuyến nghị | Căn cứ nghiệp vụ & Kỹ thuật |
|---|---|:---:|:---:|---|
| **1** | Xác thực đăng nhập & Cấp JWT Token | **Đồng bộ** | RESTful / HTTPS | Client cần token ngay lập tức để cấp quyền giao diện |
| **2** | Duyệt xem danh mục sản phẩm & Giá | **Đồng bộ** | RESTful API / CDN | Cần dữ liệu tức thời để render màn hình cho khách |
| **3** | Kiểm tra & Giữ kho trước khi đặt đơn| **Đồng bộ** | **REST / gRPC** | Order cần biết chắc chắn còn hàng; trễ thấp |
| **4** | Thanh toán qua Cổng thẻ / Ngân hàng | **Đồng bộ** | RESTful / HTTPS | Khách hàng chờ kết quả trừ tiền Thành công/Thất bại |
| **5** | Phát tín hiệu đơn hàng tạo thành công | **Bất đồng bộ** | **RabbitMQ (Fanout)** | Đơn đã lưu xong; giải phóng phản hồi ngay cho khách |
| **6** | Gửi Email / SMS xác nhận đơn hàng | **Bất đồng bộ** | RabbitMQ Queue | Gọi SMTP mất vài giây; chạy ngầm không bắt khách chờ |
| **7** | Tích điểm thành viên & Áp voucher | **Bất đồng bộ** | RabbitMQ Queue | Chấp nhận Eventual Consistency; cộng điểm sau vài giây |
| **8** | Đồng bộ dữ liệu sang Báo cáo BI | **Bất đồng bộ** | RabbitMQ Topic | Xử lý ngầm, tách biệt hoàn toàn khỏi tải giao dịch |

📚 **Nguồn tài liệu tham khảo chính thống:**
* *RabbitMQ Documentation:* rabbitmq.com/tutorials (AMQP 0-9-1 Concepts & Reliability)
* *Martin Fowler:* "MonolithFirst" (2015, martinfowler.com/bliki/MonolithFirst.html)
* *Shopify Engineering:* Kinh nghiệm thực tế vận hành Modular Monolith quy mô lớn với Packwerk
* *Eric Evans:* Domain-Driven Design: Tackling Complexity in the Heart of Software (Bounded Context)

<!-- Footer: Phụ lục tra cứu -->

<!-- Speaker Notes:
Phụ lục C phân tích chi tiết 8 luồng nghiệp vụ thực tế trong một hệ thống bán hàng và trích dẫn đầy đủ các nguồn tài liệu học thuật chính thống (Martin Fowler, RabbitMQ docs, Shopify Engineering, Eric Evans) phục vụ việc tra cứu khi hội đồng thẩm định.
-->

---

<!-- SLIDE 34: PHỤ LỤC D - NGÂN HÀNG CÂU HỎI PHẢN BIỆN -->
# PHỤ LỤC D: Ngân Hàng Câu Hỏi Phản Biện Chuyên Sâu Của Giảng Viên <span class="tag-app">[PHỤ LỤC]</span>
### Tóm lược các câu hỏi vấn đáp cốt lõi và hướng lập luận thuyết phục hội đồng

<div class="grid-2">
<div class="card-neutral">

### ❓ Nhóm Câu Hỏi Kiến Trúc (TV1 & Nhóm)
* **Câu 1: Có nhất thiết hệ thống lớn phải lên Microservices không?**  
  
 →  *Trả lời:* Hoàn toàn KHÔNG. Modular Monolith là kiến trúc đích bền vững (Shopify, Stack Overflow). Microservices không phải đích đến bắt buộc.
* **Câu 2: Chi phí ẩn lớn nhất của Microservices là gì?**  
  
 →  *Trả lời:* Khoản thuế phân tán: Chi phí hạ tầng K8s, độ trễ mạng, mất tính toàn vẹn ACID, chi phí nhân sự DevOps chuyên sâu.

</div>
<div class="card-neutral">

### ❓ Nhóm Câu Hỏi Ranh Giới & Giao Tiếp (TV2 & TV3)
* **Câu 3: Ranh giới Cart và Order có tuyệt đối duy nhất không?**  
  
 →  *Trả lời:* Không có đáp án tuyệt đối duy nhất; tách hay gộp đều hợp lý nếu căn cứ đúng Bộ 4 câu hỏi vàng.
* **Câu 4: Làm sao đảm bảo không bán vượt tồn khi 2 người cùng mua?**  
  
 →  *Trả lời:* Áp dụng Atomic Update: `UPDATE ... WHERE available >= qty` ở bước giữ hàng đồng bộ (đã chứng minh bằng `race.ps1`).

</div>
</div>

<div class="highlight">
📖 <i>Chi tiết đầy đủ 15 câu hỏi phản biện chuyên sâu xem tại tệp <code>ngan_hang_cau_hoi_phan_bien_buoi_12.md</code>.</i>
</div>

<!-- Footer: Phụ lục tra cứu -->

<!-- Speaker Notes:
Phụ lục D tóm tắt các câu hỏi phản biện kinh điển mà các Thầy Cô trong hội đồng thường đặt ra nhất:
- Microservices có phải đích đến bắt buộc không?
- Ranh giới module có tuyệt đối duy nhất không?
- Cơ chế nào chống bán vượt tồn?
Nhóm đã chuẩn bị sẵn các luận cứ đanh thép dựa trên tài liệu nghiên cứu và kết quả chạy thực tế của dự án.
-->