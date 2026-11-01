---
marp: true
theme: gaia
_class: lead
paginate: true
backgroundColor: #ffffff
header: "Phát triển Hệ thống Server Nâng cao — Buổi 12: Modular Monolithic"
footer: "Modular Monolithic — TechShop System Architecture"
style: |
  section {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
    font-size: 23px;
    padding-top: 50px;
    padding-bottom: 26px;
    padding-left: 48px;
    padding-right: 48px;
    color: #1e293b;
    background-color: #ffffff;
    line-height: 1.42;
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
  .box-order {
    background-color: #eff6ff;
    border: 1px solid #bfdbfe;
    border-left: 5px solid #2563eb;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .box-inv {
    background-color: #f0fdf4;
    border: 1px solid #bbf7d0;
    border-left: 5px solid #16a34a;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .box-notif {
    background-color: #fff7ed;
    border: 1px solid #fed7aa;
    border-left: 5px solid #ea580c;
    border-radius: 6px;
    padding: 10px 14px;
    margin: 8px 0;
  }
  .box-other {
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

<!-- _class: lead -->
<!-- _header: "" -->
<!-- _footer: "" -->
<!-- _paginate: false -->

<!-- SLIDE 1: TIÊU ĐỀ -->
<div style="text-align: center; margin-top: 120px;">

<h1 style="border-bottom: none; font-size: 30px; letter-spacing: -0.01em;">Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservice</h1>

</div>

<!-- Speaker Notes:
Kính chào quý thầy cô và các bạn. Hôm nay tôi xin trình bày chuyên đề Modular Monolithic: Tư duy thiết kế và con đường tiến hóa lên Microservice.
Bài thuyết trình tập trung vào các tiêu chí lựa chọn kiến trúc phần mềm, phương pháp phân rã nghiệp vụ theo Domain-Driven Design, và cơ chế giao tiếp đồng bộ kết hợp bất đồng bộ.
Toàn bộ lý thuyết sẽ được minh chứng thông qua hệ thống thực tế của TechShop.
Chúng ta sẽ cùng bắt đầu bằng việc tìm hiểu bài toán đặt hàng của TechShop ngay sau đây.
-->

---

<!-- SLIDE 2: BÀI TOÁN XUYÊN SUỐT: TECHSHOP -->
# TechShop: Bài Toán Đặt Hàng Trực Tuyến Và Ba Quyết Định Kiến Trúc Cốt Lõi <span class="tag-theory">[LÝ THUYẾT]</span>
### Tình huống thực tế: Khách hàng mua laptop và phụ kiện trong đợt cao điểm bán hàng

<div class="grid-3">
<div class="box-order">

### 1. Module Order
* Tiếp nhận và tạo đơn hàng.
* Chốt dữ liệu đơn trạng thái `Confirmed`.
* Quản lý thông tin thanh toán đơn.

</div>
<div class="box-inv">

### 2. Module Inventory
* Quản lý số lượng tồn kho.
* Kiểm tra và giữ hàng nguyên tử (`Reserve`).
* Xác nhận đã bán (`Sold`).

</div>
<div class="box-notif">

### 3. Module Notification
* Tạo bản ghi thông báo.
* Gửi email mô phỏng cho khách.
* Chạy ngầm độc lập.

</div>
</div>

```
[Khách hàng] ──(1. POST /api/orders)──> [Order Module] ──(2. ReserveStock)──> [Inventory Module]
                                              |
                                              + - - - (3. OrderCreated) - - - > [Notification Module]
```

* **Ba câu hỏi kiến trúc cốt lõi:** Chọn kiến trúc nào phù hợp nguồn lực? Phân rã module ra sao để tránh phụ thuộc? Các module giao tiếp với nhau bằng cơ chế gì?

<!-- Speaker Notes:
Hệ thống mẫu xuyên suốt bài thuyết trình là TechShop, nền tảng thương mại điện tử chuyên bán laptop và phụ kiện.
Hệ thống gồm đúng ba module nghiệp vụ: Order chịu trách nhiệm tạo và lưu đơn hàng; Inventory chịu trách nhiệm kiểm tra tồn, giữ hàng và xác nhận bán; Notification chịu trách nhiệm ghi và gửi thông báo mô phỏng cho khách.
Khi khách đặt hàng, Order gọi đồng bộ sang Inventory để giữ hàng, sau đó lưu đơn và phát sự kiện OrderCreated bất đồng bộ cho Notification xử lý.
Để xây dựng hệ thống này ổn định, chúng ta phải trả lời ba câu hỏi lớn: chọn kiến trúc nào, chia module thế nào, và giao tiếp ra sao.
Trước hết, chúng ta hãy so sánh ba mô hình kiến trúc phổ biến hiện nay.
-->

---

<!-- SLIDE 3: MONOLITHIC -->
# Kiến Trúc Monolithic: Phát Triển Đơn Giản Nhưng Dễ Gặp Điểm Nghẽn Mở Rộng <span class="tag-theory">[LÝ THUYẾT]</span>
### Toàn bộ ứng dụng đóng gói trong một tiến trình thực thi và chia sẻ một cơ sở dữ liệu

* **Bản chất kiến trúc:** Toàn bộ mã nguồn nằm trong một codebase, chạy trong một tiến trình hệ điều hành (process), và thường dùng chung một cơ sở dữ liệu.
* **Ưu điểm thực tế:**
  * Quy trình phát triển, biên dịch, kiểm thử tự động và triển khai rất đơn giản.
  * Phù hợp với đội ngũ kỹ sư quy mô nhỏ và sản phẩm đang ở giai đoạn ban đầu.
  * Lời gọi hàm diễn ra trực tiếp trong bộ nhớ RAM, bảo toàn tính nhất quán ACID dễ dàng.
* **Nhược điểm khi quy mô tăng:**
  * Mã nguồn dễ rối dần thành spaghetti code nếu thiếu kỷ luật kiến trúc nghiêm ngặt.
  * Khó mở rộng (scale) độc lập cho riêng các module chịu tải cao; lỗi ở một vị trí có thể ảnh hưởng toàn bộ ứng dụng; nhiều nhóm cùng sửa một codebase dễ xung đột mã nguồn.
* **Minh họa TechShop:** Toàn bộ chức năng đặt hàng, quản lý kho và gửi thông báo nằm chung trong một ứng dụng duy nhất và đọc ghi chung các bảng CSDL.

<!-- Speaker Notes:
Mô hình đầu tiên là Monolithic truyền thống.
Bản chất của kiến trúc này là một codebase duy nhất, chạy trong một tiến trình duy nhất và dùng chung một cơ sở dữ liệu.
Ưu điểm lớn nhất là sự đơn giản: đội ngũ nhỏ có thể phát triển, kiểm thử và triển khai rất nhanh mà không cần hạ tầng phức tạp.
Tuy nhiên, khi quy mô mã nguồn và lượng truy cập tăng lên, Monolith bộc lộ nhược điểm lớn: khó scale riêng module kho hàng trong mùa sale, và lỗi phát sinh ở một chức năng có thể kéo sập toàn bộ hệ thống.
Để giải quyết bài toán scale độc lập, ngành công nghệ đã phát triển mô hình Microservices.
-->

---

<!-- SLIDE 4: MICROSERVICES -->
# Kiến Trúc Microservices: Mở Rộng Linh Hoạt Nhưng Chi Phí Vận Hành Rất Lớn <span class="tag-theory">[LÝ THUYẾT]</span>
### Mỗi nghiệp vụ được tách thành một dịch vụ độc lập với cơ sở dữ liệu và quy trình triển khai riêng

* **Bản chất kiến trúc:** Mỗi nghiệp vụ là một dịch vụ độc lập, sở hữu cơ sở dữ liệu riêng, triển khai độc lập, mở rộng độc lập và có thể dùng công nghệ khác nhau.
* **Ưu điểm chiến lược:**
  * Mở rộng tài nguyên độc lập cho đúng dịch vụ chịu tải cao mà không lãng phí chi phí phần cứng.
  * Các nhóm kỹ sư có thể phát triển, kiểm thử và release song song mà không phụ thuộc lẫn nhau.
  * Dễ dàng nâng cấp hoặc thay thế công nghệ cho từng phần nhỏ của hệ thống.
* **Nhược điểm và thách thức vận hành:**
  * Cần hệ thống CI/CD riêng cho từng service, cơ chế service discovery, cùng hạ tầng giám sát và ghi log tập trung.
  * Phải xử lý độ trễ mạng và các bài toán lỗi phân tán; quản lý giao dịch qua nhiều dịch vụ phức tạp hơn rất nhiều so với giao dịch CSDL thông thường; đòi hỏi đội DevOps đủ mạnh.
* **Minh họa TechShop:** Order, Inventory và Notification là 3 ứng dụng riêng biệt, chạy trên 3 máy chủ hoặc container khác nhau, dùng 3 database riêng và 3 pipeline CI/CD riêng.

<!-- Speaker Notes:
Ở thái cực đối lập, Microservices phân chia mỗi nghiệp vụ thành một ứng dụng độc lập với database riêng biệt.
Ưu điểm rõ rệt là khả năng mở rộng linh hoạt: dịch vụ kho hàng có thể scale lên nhiều bản sao khi tải tăng đột biến mà không ảnh hưởng tới dịch vụ khác.
Tuy nhiên, cái giá phải trả là độ phức tạp vận hành tăng lên rất cao: cần hạ tầng CI/CD riêng cho từng service, hệ thống giám sát phân tán, và xử lý bài toán mất tính nhất quán giao dịch ACID giữa các service.
Một đội ngũ nhỏ nếu vội vã chọn Microservices sẽ dễ bị quá tải vận hành.
Vậy có giải pháp nào kết hợp được sự đơn giản của Monolith với tính ngăn nắp của Microservices?
-->

---

<!-- SLIDE 5: MODULAR MONOLITHIC NẰM Ở GIỮA -->
# Modular Monolithic: Điểm Cân Bằng Giữa Vận Hành Đơn Khối Và Ranh Giới Nghiệp Vụ <span class="tag-theory">[LÝ THUYẾT]</span>
### Một đơn vị triển khai duy nhất nhưng mã nguồn được tổ chức thành các module khép kín

* **Bản chất kỹ thuật:** Vẫn là một codebase, một tiến trình thực thi, một lần triển khai (deploy); nhưng mã nguồn được phân chia nghiêm ngặt thành các module độc lập theo nghiệp vụ, giao tiếp qua interface thay vì gọi chéo tùy tiện.
* **Ưu điểm dung hòa:** Vận hành đơn giản với một pipeline CI/CD duy nhất, nhưng kỷ luật tổ chức mã nguồn gần với Microservices; cho phép bóc tách dần từng module thành service riêng khi cần mà không phải viết lại từ đầu.

| Tiêu chí kỹ thuật | Monolithic truyền thống | Modular Monolithic | Microservices Architecture |
|---|---|---|---|
| **Số codebase** | 1 Repository | 1 Repository | Đa Repository hoặc Monorepo |
| **Đơn vị Deployment** | 1 Process / 1 đơn vị deploy | 1 Process / 1 đơn vị deploy | Hàng chục Container / Pods riêng |
| **Cơ sở dữ liệu** | 1 Database chung bảng | 1 Database tách riêng Schema | Database-per-Service riêng biệt |
| **Cách giao tiếp module** | Gọi hàm nội bộ trực tiếp | Qua Public Interface trong RAM | Lời gọi mạng (REST, gRPC, MQ) |
| **Độ phức tạp vận hành** | Rất thấp | Thấp | Rất cao |
| **Khả năng bóc tách về sau**| Rất khó khăn do dính chặt | Thuận lợi nhờ interface có sẵn | Không cần tách (đã tách sẵn) |

* **Minh họa TechShop:** Ứng dụng demo hôm nay được triển khai hoàn toàn theo kiến trúc Modular Monolithic.

<!-- Speaker Notes:
Modular Monolithic xuất hiện như một điểm cân bằng tối ưu giữa hai thái cực.
Hệ thống vẫn giữ được sự tinh gọn trong vận hành: một codebase, một tiến trình duy nhất, và một pipeline CI/CD.
Đồng thời, mã nguồn được chia thành các module có ranh giới rõ ràng, giao tiếp qua interface trừu tượng, và cơ sở dữ liệu được phân chia theo từng schema độc lập.
Bảng so sánh cho thấy Modular Monolith mang lại chi phí vận hành thấp như Monolith, nhưng lại sẵn sàng cho việc bóc tách thành Microservices khi có nhu cầu thực tế.
Tiếp theo, chúng ta cùng xem xét ba tiêu chí cụ thể để lựa chọn giữa các kiến trúc này.
-->

---

<!-- SLIDE 6: CHỌN KIẾN TRÚC THEO 3 TIÊU CHÍ -->
# Ba Tiêu Chí Quyết Định Kiến Trúc: Đội Ngũ, Năng Lực Vận Hành Và Giai Đoạn Sản Phẩm <span class="tag-theory">[LÝ THUYẾT]</span>
### Lựa chọn kiến trúc dựa trên nguồn lực thực tế thay vì áp dụng theo trào lưu

<div class="grid-3">
<div class="box-other">

### 1. Quy Mô Đội Ngũ
* Đội ngũ nhỏ (vài người): Monolithic hoặc Modular Monolith dễ quản lý hơn nhiều lần.
* Nhiều team độc lập phụ trách từng mảng: Microservices giúp các nhóm triển khai song song.

</div>
<div class="box-other">

### 2. Năng Lực Vận Hành
* Microservices đòi hỏi hạ tầng và đội ngũ DevOps đủ mạnh.
* Thiếu nguồn lực này dễ dẫn đến gánh nặng vận hành trước khi kịp hưởng lợi ích mở rộng.

</div>
<div class="box-other">

### 3. Giai Đoạn Sản Phẩm
* Sản phẩm mới, ranh giới chưa ổn định: Modular Monolith giúp tái cấu trúc ranh giới nhanh.
* Sản phẩm trưởng thành, điểm nghẽn rõ: Bóc tách đúng module cần scale độc lập.

</div>
</div>

| Giai đoạn phát triển của TechShop | Đánh giá ba tiêu chí | Quyết định kiến trúc lựa chọn |
|---|---|---|
| **Giai đoạn đầu (Đội ngũ nhỏ, tính năng thay đổi)** | Ít kỹ sư, chưa có chuyên trách DevOps, ranh giới chưa cố định | **Modular Monolithic** (tiết kiệm chi phí, dễ chỉnh sửa) |
| **Giai đoạn sau (Tình huống giả định: Mùa sale tải cao)**| Đội ngũ lớn hơn, module Inventory quá tải cục bộ | **Tách riêng module Inventory** thành service độc lập |

<!-- Speaker Notes:
Để chọn đúng kiến trúc, doanh nghiệp cần đánh giá dựa trên ba tiêu chí thực tế.
Tiêu chí thứ nhất là quy mô đội ngũ: nhóm nhỏ nên chọn Modular Monolith để tập trung vào tính năng thay vì hạ tầng.
Tiêu chí thứ hai là năng lực vận hành: nếu chưa có đội ngũ DevOps mạnh, việc triển khai Microservices sẽ gây quá tải vận hành.
Tiêu chí thứ ba là giai đoạn sản phẩm: ở giai đoạn đầu, ranh giới nghiệp vụ liên tục thay đổi, Modular Monolith cho phép tái cấu trúc nhanh nhất.
Bảng so sánh cho thấy TechShop ở giai đoạn đầu chọn Modular Monolith, và chỉ khi Inventory bị quá tải trong mùa sale thì mới bóc tách riêng.
-->

---

<!-- SLIDE 7: MONOLITHFIRST VÀ KIẾN TRÚC LAI -->
# Bắt Đầu Bằng Modular Monolith Và Duy Trì Kiến Trúc Lai Khi Tiến Hóa <span class="tag-theory">[LÝ THUYẾT]</span>
### Nguyên tắc MonolithFirst — Tránh bóc tách toàn bộ hệ thống cùng một lúc

* **Nguyên tắc MonolithFirst:** Luôn bắt đầu hệ thống bằng Modular Monolithic; chỉ tách một module thành service độc lập khi có **lý do thực tế rõ ràng**:
  * Nhóm phát triển cần làm việc và release hoàn toàn tách biệt.
  * Module cần mở rộng tài nguyên (scale) riêng biệt do tải cao.
  * Module yêu cầu công nghệ đặc thù hoặc có yêu cầu cô lập về an ninh dữ liệu / compliance.
* **Không tách toàn bộ cùng lúc:** Tránh việc bóc tách toàn bộ hệ thống trong một lần duy nhất; kiến trúc lai (Hybrid Architecture) kết hợp giữa monolith lõi và một số service vệ tinh là mô hình rất phổ biến trong thực tế.

```
+-------------------------------------------------------------+
|               TECHSHOP CORE MODULAR MONOLITH                |
|      (Giữ nguyên các module: Order, Notification,...)       |
+-------------------------------------------------------------+
                              |
                              | Lời gọi qua mạng (REST / gRPC)
                              v
                +----------------------------+
                |     INVENTORY SERVICE      |
                |   (Tách thành service riêng|
                |    để chịu tải mùa sale)   |
                +----------------------------+
```

<div class="warning-box" style="text-align: center; font-weight: bold;">
LƯU Ý CỐT LÕI: "Microservices không phải đích đến bắt buộc của mọi hệ thống."
</div>

<!-- Speaker Notes:
Nguyên tắc MonolithFirst chỉ ra rằng chúng ta nên bắt đầu bằng Modular Monolith để xác định đúng ranh giới nghiệp vụ trước khi chia nhỏ.
Chúng ta chỉ bóc tách một module khi có lý do rõ ràng: như yêu cầu scale riêng, yêu cầu công nghệ, hoặc tuân thủ an ninh.
Không cần thiết phải chuyển đổi 100% sang Microservices; mô hình kiến trúc lai giữ các module ổn định trong lõi và chỉ đưa Inventory ra ngoài là cách tiếp cận phổ biến.
Cần khẳng định rõ: Microservices không phải là đích đến bắt buộc của mọi phần mềm.
Vậy khi có nhu cầu bóc tách một module, quy trình kỹ thuật sẽ diễn ra như thế nào?
-->

---

<!-- SLIDE 8: BỐC TÁCH MODULE CẦN 4 BƯỚC -->
# Bóc Tách Một Module Thành Service Độc Lập Thực Hiện Qua 4 Bước Chuẩn <span class="tag-theory">[LÝ THUYẾT]</span>
### Điều kiện tiên quyết: Module đã sở hữu schema CSDL riêng và giao tiếp qua Public Interface

```
[ BƯỚC 1: TÁCH DATABASE RIÊNG ]
Tách schema của module sang một cơ sở dữ liệu vật lý riêng biệt
               |
               v
[ BƯỚC 2: THAY LỜI GỌI NỘI BỘ BẰNG MẠNG ]
Thay interface in-process bằng lời gọi qua network: Đồng bộ (REST/gRPC) hoặc Bất đồng bộ (RabbitMQ)
               |
               v
[ BƯỚC 3: ĐÓNG GÓI DOCKER IMAGE RIÊNG ]
Đóng gói module thành container độc lập (áp dụng kỹ thuật container hóa Buổi 6, 7)
               |
               v
[ BƯỚC 4: THIẾT LẬP PIPELINE CI/CD RIÊNG ]
Xây dựng quy trình tự động build, test và deploy riêng biệt cho service mới (kế thừa Buổi 10, 11)
```

* **Yêu cầu thực thi:** Việc bóc tách chỉ thực hiện được thuận lợi khi module đã được thiết kế cô lập schema và dùng interface từ ban đầu.
* **Chiến lược áp dụng:** Tách dần từng module có lý do rõ rệt nhất trước, ví dụ: Payment (yêu cầu an ninh PCI-DSS) hoặc Inventory (yêu cầu scale tải cao).

<!-- Speaker Notes:
Khi cần bóc tách một module như Inventory ra thành microservice độc lập, kỹ sư thực hiện theo đúng 4 bước kỹ thuật.
Bước 1: Tách schema dữ liệu sang máy chủ database vật lý riêng.
Bước 2: Thay thế lời gọi interface trong bộ nhớ bằng lời gọi qua mạng, có thể là REST, gRPC hoặc RabbitMQ.
Bước 3: Đóng gói module thành Docker image riêng biệt.
Bước 4: Thiết lập pipeline CI/CD độc lập để triển khai tự động.
Điều kiện bắt buộc để làm được 4 bước này là module đã được cô lập dữ liệu từ trước.
Bây giờ chúng ta sẽ bước vào Phần 2: Tư duy phân rã nghiệp vụ để thiết lập ranh giới module đúng đắn.
-->

---

<!-- SLIDE 9: CHIA MODULE THEO NGHIỆP VỤ -->
# Phân Chia Module Theo Ranh Giới Nghiệp Vụ Thay Vì Chia Theo Cảm Tính <span class="tag-theory">[LÝ THUYẾT]</span>
### Domain Decomposition: Cơ sở thiết lập ranh giới độc lập và bền vững cho hệ thống

* **Khái niệm Domain Decomposition:** Tách bài toán nghiệp vụ lớn thành các module tương ứng với các ranh giới nghiệp vụ thực tế trong doanh nghiệp.
* **Hậu quả của phân chia theo cảm tính:** Nhóm các lớp theo kỹ thuật (ví dụ gom chung vào `Utils`, `Helpers`, `Services`) dẫn đến việc chia sai ranh giới, các thành phần phụ thuộc chéo lẫn nhau và không thể bóc tách thành service về sau.

```
CẤU TRÚC THEO KỸ THUẬT (SAI - KHÓ TÁCH)        CẤU TRÚC THEO NGHIỆP VỤ (ĐÚNG NHƯ DEMO)
├── Controllers/                              ├── Modules/
│   ├── OrderController.cs                    │   ├── Order/
│   ├── InventoryController.cs                │   │   ├── Domain, Application, Infrastructure
│   └── NotificationController.cs             │   ├── Inventory/
├── Services/                                 │   │   ├── Domain, Application, Infrastructure
│   └── (Gọi chéo phụ thuộc vòng)             │   └── Notification/
└── Data/                                     │       └── Domain, Application, Infrastructure
    └── AppDbContext.cs (Dùng chung bảng)     └── Shared/
```

* **Kết luận thiết kế:** Mỗi module phải quản lý trọn vẹn nghiệp vụ của mình từ tầng xử lý đến tầng dữ liệu.

<!-- Speaker Notes:
Phần 2 là phần trọng tâm nhất của buổi học: Tư duy phân rã nghiệp vụ.
Domain Decomposition là phương pháp chia nhỏ bài toán hệ thống dựa trên ranh giới nghiệp vụ thực tế.
Nếu chia theo cảm tính hoặc chia theo tầng kỹ thuật như tạo thư mục Controllers hay Services dùng chung, mã nguồn sẽ bị phụ thuộc chéo chằng chịt và không thể tách ra sau này.
Ngược lại, khi chia theo chiều dọc nghiệp vụ như TechShop: Order, Inventory và Notification là ba module độc lập khép kín từ tầng ứng dụng đến cơ sở dữ liệu.
Để hiểu rõ cách phân định này, chúng ta cần tìm hiểu khái niệm Bounded Context.
-->

---

<!-- SLIDE 10: BOUNDED CONTEXT -->
# Bounded Context: Mỗi Module Bao Trùm Ngữ Cảnh Và Ngôn Ngữ Nghiệp Vụ Riêng <span class="tag-theory">[LÝ THUYẾT]</span>
### Cùng khái niệm "Sản phẩm", mỗi module định nghĩa các thuộc tính và quy tắc khác nhau

* **Khái niệm Bounded Context (Domain-Driven Design):** Mỗi module bao trùm một ngữ cảnh nghiệp vụ trọn vẹn, có khái niệm và ngôn ngữ nghiệp vụ riêng (Ubiquitous Language), hạn chế tối đa việc phụ thuộc vào chi tiết cài đặt của module khác.

<div class="grid-2">
<div class="box-other">

### 1. Catalog Context
* Khái niệm: Sản phẩm chào bán trên web.
* Thuộc tính: Tên, mô tả, hình ảnh, thông số kỹ thuật, giá niêm yết.

</div>
<div class="box-inv">

### 2. Inventory Context
* Khái niệm: Mặt hàng lưu trữ trong kho.
* Thuộc tính: Mã SKU, số lượng tồn khả dụng, số lượng đang giữ, số lượng đã bán.

</div>
</div>

<div class="grid-2">
<div class="box-other">

### 3. Cart Context
* Khái niệm: Mặt hàng khách tạm thời lựa chọn.
* Thuộc tính: ProductId tham chiếu, số lượng chọn mua, phiên truy cập (Session).

</div>
<div class="box-order">

### 4. Order Context
* Khái niệm: Dòng mục trong đơn hàng chính thức.
* Thuộc tính: Bản chụp tên sản phẩm (`ProductNameSnapshot`), đơn giá chốt tại thời điểm mua (`UnitPriceSnapshot`), số lượng.

</div>
</div>

<!-- Speaker Notes:
Khái niệm cốt lõi mượn từ Domain-Driven Design là Bounded Context.
Mỗi module tạo ra một ngữ cảnh nghiệp vụ hoàn chỉnh với ngôn ngữ riêng.
Ví dụ rõ nhất tại TechShop là khái niệm Sản phẩm:
Tại Catalog, Sản phẩm là thông tin chào bán gồm tiêu đề, mô tả và hình ảnh.
Tại Inventory, Sản phẩm là đơn vị tồn kho với mã SKU và số lượng khả dụng.
Tại Cart, Sản phẩm là lựa chọn tạm thời trong giỏ hàng.
Còn tại Order, Sản phẩm bắt buộc phải là một Bản chụp bất biến lưu lại tên và giá tại đúng thời điểm khách bấm mua, không được phụ thuộc vào giá Catalog sau đó.
-->

---

<!-- SLIDE 11: 10 NGHIỆP VỤ E-COMMERCE -->
# Hệ Thống E-Commerce: Phân Định 8 Module Lõi Và 2 Module Mở Rộng <span class="tag-theory">[LÝ THUYẾT]</span>
### Xác định phạm vi triển khai thực tế trên bài toán thương mại điện tử TechShop

* **10 Nghiệp vụ E-Commerce tổng thể:**
  1. **Catalog** (Quản lý sản phẩm chào bán)
  2. **Cart** (Giỏ hàng người dùng)
  3. **Order** (Xử lý đơn hàng chính thức)
  4. **Payment** (Giao dịch thanh toán)
  5. **Inventory** (Quản lý tồn kho)
  6. **Shipping** (Giao nhận vận chuyển)
  7. **Identity** (Xác thực và phân quyền)
  8. **Notification** (Gửi thông báo Email/SMS)
  9. *Promotion* (Khuyến mãi và mã giảm giá — Mảng mở rộng)
  10. *Review* (Đánh giá và nhận xét — Mảng mở rộng)
* **Tám (8) Module lõi theo đề bài:** **Catalog, Cart, Order, Payment, Inventory, Shipping, Identity, Notification**.
* **Ba (3) Module trọng tâm demo triển khai:** **Order, Inventory, Notification**.

<div class="warning-box" style="text-align: center; font-weight: bold;">
LƯU Ý CỐT LÕI: "Không có đáp án duy nhất; quan trọng là giải thích được vì sao chia như vậy."
</div>

<!-- Speaker Notes:
Trong bài toán thương mại điện tử, chúng ta phân tích 10 nghiệp vụ điển hình.
Tài liệu học phần xác định 8 module lõi bao gồm: Catalog, Cart, Order, Payment, Inventory, Shipping, Identity và Notification. Hai module Promotion và Review thuộc mảng mở rộng.
Trong buổi hôm nay, dự án demo tập trung hiện thực hóa trọn vẹn 3 module đại diện cốt lõi: Order, Inventory và Notification.
Cần khẳng định rõ: không có một cách chia module tuyệt đối duy nhất; điều quan trọng là nhóm kỹ sư phải giải thích được căn cứ của việc phân chia.
Căn cứ đó chính là Bộ 4 câu hỏi định lượng mà chúng ta sẽ tìm hiểu tiếp theo.
-->

---

<!-- SLIDE 12: BỘ 4 CÂU HỎI -->
# Bộ Bốn Câu Hỏi Định Lượng Giúp Xác Định Ranh Giới Module Khách Quan <span class="tag-theory">[LÝ THUYẾT]</span>
### Áp dụng bốn tiêu chí để đánh giá từng nghiệp vụ trước khi quyết định tách module

<div class="grid-2">
<div class="box-other">

### 1. Ngôn ngữ nghiệp vụ riêng?
* Nghiệp vụ có khái niệm và thuật ngữ riêng biệt không?
* Ví dụ: SKU, tồn khả dụng, vị trí kho là ngôn ngữ riêng của Inventory; khác hoàn toàn với trạng thái thanh toán hay mã đơn hàng.

</div>
<div class="box-other">

### 2. Trách nhiệm trong tổ chức?
* Bộ phận nào trong tổ chức thực tế chịu trách nhiệm vận hành?
* Ví dụ: Bộ phận Kho vận chịu trách nhiệm hàng hóa; bộ phận Kế toán quản lý thanh toán; bộ phận Chăm sóc khách hàng quản lý thông báo.

</div>
</div>

<div class="grid-2">
<div class="box-other">

### 3. Tính độc lập của dữ liệu?
* Dữ liệu có thể đứng độc lập mà không cần thực hiện truy vấn JOIN trực tiếp với module khác không?
* Cần loại bỏ hoàn toàn việc JOIN bảng dữ liệu giữa các module khác nhau.

</div>
<div class="box-other">

### 4. Tốc độ thay đổi yêu cầu?
* Tần suất thay đổi yêu cầu nghiệp vụ có khác nhau không?
* Ví dụ: Chính sách khuyến mãi (Promotion) thay đổi liên tục theo chiến dịch tuần; trong khi cấu trúc Catalog tương đối ổn định theo năm.

</div>
</div>

<!-- Speaker Notes:
Để không chia module theo cảm tính, chúng ta áp dụng Bộ 4 câu hỏi vàng để đánh giá từng nghiệp vụ.
Câu hỏi 1: Nghiệp vụ có ngôn ngữ riêng không?
Câu hỏi 2: Bộ phận nào trong tổ chức chịu trách nhiệm? Ranh giới tổ chức thường phản ánh đúng ranh giới phần mềm.
Câu hỏi 3: Dữ liệu có thể lưu trữ độc lập mà không cần lệnh SQL JOIN chéo bảng hay không?
Câu hỏi 4: Tốc độ thay đổi yêu cầu có khác biệt không? Module nào thay đổi nhanh cần tách riêng để tránh làm ảnh hưởng module ổn định.
Bây giờ, chúng ta áp dụng 4 câu hỏi này vào trường hợp cụ thể của TechShop.
-->

---

<!-- SLIDE 13: ÁP DỤNG 4 CÂU HỎI VÀO TECHSHOP -->
# Áp Dụng Bộ 4 Câu Hỏi Vào Hệ Thống TechShop Để Quyết Định Ranh Giới Module <span class="tag-theory">[LÝ THUYẾT]</span>
### Bảng phân tích 4 module tiêu biểu — Tách hay gộp đều hợp lý nếu có căn cứ rõ ràng

| Nghiệp vụ khảo sát | 1. Ngôn ngữ riêng | 2. Trách nhiệm tổ chức | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định module |
|---|---|---|---|---|:---:|
| **Inventory (Kho)** | Có (SKU, tồn khả dụng) | Bộ phận Kho vận | Có (Quản lý theo SKU) | Thấp | **Tách riêng** |
| **Promotion (Khuyến mãi)** | Có (Voucher, giảm giá) | Bộ phận Marketing | Có (Tính tiền giảm qua DTO)| **Rất cao** | **Tách riêng** |
| **Payment (Thanh toán)** | Có (Giao dịch, đối soát) | Bộ phận Kế toán - Tài chính| Có (Lưu log đối soát mã) | Thấp | **Tách riêng** |
| **Notification (Thông báo)** | Có (Kênh gửi, template) | Bộ phận CSKH | Có (Nhận dữ liệu từ sự kiện) | Trung bình | **Tách riêng** |

<div class="warning-box">
LƯU Ý: Cart và Order có thể tách riêng hoặc gộp chung tùy vào quy mô hệ thống, cả hai phương án đều được chấp nhận nếu nhóm giải thích được căn cứ theo 4 câu hỏi trên. <i>(Ma trận đầy đủ 10 nghiệp vụ xem tại Phụ lục A).</i>
</div>

<!-- Speaker Notes:
Bảng này minh họa việc áp dụng Bộ 4 câu hỏi vào 4 module tiêu biểu của TechShop.
Inventory được tách riêng vì ngôn ngữ kho vận đặc thù và thuộc bộ phận Kho.
Promotion được tách riêng vì tần suất thay đổi rất cao, việc sửa đổi khuyến mãi không được làm ảnh hưởng tới xử lý đơn hàng.
Payment và Notification đều thỏa mãn tính độc lập về dữ liệu và trách nhiệm tổ chức.
Cần lưu ý: việc tách hay gộp giữa Cart và Order đều hợp lý tùy theo quy mô của TechShop; quan trọng là nhóm kỹ sư giải thích được nguyên nhân.
Tiếp theo, chúng ta cùng xem xét cấu trúc mã nguồn bên trong một module được tổ chức như thế nào.
-->

---

<!-- SLIDE 14: CẤU TRÚC BÊN TRONG MODULE -->
# Cấu Trúc Bên Trong Module: Clean Architecture Thu Nhỏ Và Schema CSDL Độc Lập <span class="tag-theory">[LÝ THUYẾT]</span>
### Kế thừa kiến trúc phân tầng Buổi 4 và phân tách schema trên PostgreSQL thật

<div class="grid-2">
<div>

### Cấu Trúc Mã Nguồn Module
* Mỗi module là một project hoặc thư mục riêng:
  * `Domain`: Thực thể cốt lõi (`Stock.cs`).
  * `Application`: Use cases điều phối (`InProcessInventoryService.cs`).
  * `Infrastructure`: `InventoryDbContext.cs` (**khai báo internal**).
  * `Contracts`: Public interface và DTO công khai (`Inventory.Contracts`).

</div>
<div>

### Phân Tách Schema Trên PostgreSQL
* 3 schema độc lập trên cùng 1 server PostgreSQL:
  * Schema `inventory`: Bảng `stocks`, `processed_events`.
  * Schema `order`: Bảng `orders`, `order_items`.
  * Schema `notification`: Bảng `notification_logs`, `processed_events`.
* Tên schema khớp chính xác tệp `init-db.sql` của dự án.

</div>
</div>

```
demo/src/Modules/
├── Inventory.Contracts/   <-- Public interface & DTOs công khai cho module khác dùng
├── Inventory/             <-- Quản lý tồn kho (Internal DbContext, schema 'inventory')
├── Order/                 <-- Quản lý đơn hàng (Internal DbContext, schema 'order')
└── Notification/          <-- Gửi thông báo (Internal DbContext, schema 'notification')
```

<!-- Speaker Notes:
Mỗi module bên trong Modular Monolith được tổ chức theo kiến trúc Clean Architecture thu nhỏ đã học từ Buổi 4.
Cấu trúc gồm các tầng Domain, Application, Infrastructure và Contracts.
Điểm then chốt ở tầng dữ liệu là mỗi module sở hữu một schema độc lập trên cùng một máy chủ PostgreSQL: schema inventory, order và notification.
Các bảng được tạo đúng theo kịch bản trong file init-db.sql của dự án demo.
Tầng Contracts được tách riêng để cung cấp interface công khai cho các module khác gọi tới mà không để lộ chi tiết nội bộ.
Vậy các module phải tuân thủ những quy tắc giao tiếp nào?
-->

---

<!-- SLIDE 15: QUY TẮC GIAO TIẾP VÀ CÔ LẬP -->
# Quy Tắc Giao Tiếp Giữa Các Module: Khóa Chặt CSDL Và Chỉ Qua Public Interface <span class="tag-theory">[LÝ THUYẾT]</span>
### Ngăn chặn lỗi vi phạm kiến trúc phổ biến nhất bằng kiểm thử tự động NetArchTest

* **Quy tắc cô lập cơ sở dữ liệu:**
  * Module A không được query trực tiếp vào bảng dữ liệu của Module B.
  * Module A không được inject hay gọi trực tiếp vào `DbContext` hoặc `Repository` nội bộ của Module B.
* **Giao tiếp qua Public Interface:**
  * Module B công khai một interface dịch vụ (ví dụ: `IInventoryService`); Module A chỉ được gọi thông qua interface này.
  * Interface công khai đóng vai trò là "đường cắt kiến trúc" (architectural seam); khi tách module thành microservice sau này, lời gọi qua interface sẽ được thay thế bằng lời gọi mạng mà logic nghiệp vụ bên trong không phải viết lại.
* **Lỗi phổ biến nhất:** Module truy cập dữ liệu chéo của nhau khiến hệ thống mang danh nghĩa "Modular Monolithic" nhưng thực chất vẫn là monolith rối rắm, không thể bóc tách về sau.

```
[Order Module] ──Gọi qua Public Interface──> [IInventoryService] ──Thực thi nội bộ──> [Inventory Module]
      |                                                                                    |
  (Không được truy cập) ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── ── > [InventoryDbContext: internal]
```

<div class="highlight">
Bảo đảm kỷ luật: Dự án demo sử dụng bài test kiến trúc tự động (NetArchTest) để kiểm tra việc không vi phạm quy tắc này.
</div>

<!-- Speaker Notes:
Để bảo vệ ranh giới module, chúng ta phải tuân thủ tuyệt đối quy tắc giao tiếp.
Thứ nhất, cấm tuyệt đối việc module này query thẳng vào bảng hoặc inject DbContext của module khác.
Thứ hai, mọi giao tiếp bắt buộc phải đi qua Public Interface như IInventoryService.
Interface này là đường cắt kiến trúc: khi còn chạy chung thì gọi trong RAM, khi tách ra microservice thì đổi sang gọi mạng mà không cần sửa code nghiệp vụ.
Lỗi phổ biến nhất của các dự án thất bại là để các module gọi chéo database của nhau.
Trong demo hôm nay, nhóm đã thiết lập bài test NetArchTest tự động để bảo đảm tính kỷ luật này.
Bây giờ, chúng ta chuyển sang Phần 3: Cơ chế giao tiếp đồng bộ và bất đồng bộ giữa các module.
-->

---

<!-- SLIDE 16: GIAO TIẾP ĐỒNG BỘ: REST VÀ GRPC -->
# Giao Tiếp Đồng Bộ: Áp Dụng Khi Cần Kết Quả Ngay Lập Tức Để Quyết Định <span class="tag-theory">[LÝ THUYẾT]</span>
### Cơ chế bên gọi chờ phản hồi — So sánh công nghệ RESTful API và gRPC

* **Bản chất giao tiếp đồng bộ:** Bên gọi gửi yêu cầu và tạm dừng luồng logic để chờ phản hồi rồi mới tiếp tục xử lý.
* **Áp dụng tại TechShop:** Order bắt buộc phải gọi đồng bộ sang Inventory để kiểm tra và giữ hàng trước khi xác nhận tạo đơn hàng cho khách.

| Tiêu chí kỹ thuật | RESTful API (HTTP/1.1 + JSON) | gRPC (HTTP/2 + Protocol Buffers) |
|---|---|---|
| **Định dạng dữ liệu** | Dạng văn bản JSON dễ đọc bằng mắt, dễ debug bằng Postman | Nhị phân nén nhỏ gọn, hợp đồng strongly-typed qua file `.proto` |
| **Giao thức mạng** | HTTP/1.1 (mỗi request mở kết nối riêng) | HTTP/2 Multiplexing (ghép nhiều luồng trên 1 kết nối TCP) |
| **Phù hợp sử dụng** | Public API cho Web Frontend, Mobile App hoặc đối tác ngoài | Giao tiếp nội bộ tốc độ cao giữa các backend service |

<div class="warning-box">
MINH BẠCH KỸ THUẬT: Demo sử dụng REST qua HttpClient loopback (cùng process); gRPC là phương án công nghệ khuyến nghị khi bóc tách Inventory thành microservice riêng biệt.
</div>

<!-- Speaker Notes:
Bắt đầu Phần 3 về giao tiếp giữa các module:
Giao tiếp đồng bộ là cơ chế mà bên gọi phải đứng chờ kết quả phản hồi để quyết định bước tiếp theo.
Tại TechShop, Order bắt buộc phải gọi đồng bộ sang Inventory để kiểm tra và giữ hàng trước khi chốt đơn.
So sánh giữa hai công nghệ: RESTful API dùng JSON dễ đọc, phù hợp cho API công khai ra ngoài; còn gRPC dùng HTTP/2 và nhị phân Protobuf, phù hợp cho giao tiếp nội bộ giữa các service backend.
Nhóm xin minh bạch: trong bản demo hôm nay, nhóm triển khai REST gọi loopback qua HttpClient; gRPC là phương án lựa chọn khi tách service.
Tuy nhiên, gọi đồng bộ tiềm ẩn một nguy cơ rất lớn cần lưu ý.
-->

---

<!-- SLIDE 17: CASCADING FAILURE -->
# Rủi Ro Của Chuỗi Gọi Đồng Bộ: Lỗi Dây Chuyền Kéo Sập Toàn Bộ Hệ Thống <span class="tag-theory">[LÝ THUYẾT]</span>
### Tình huống một dịch vụ bị chậm hoặc sập sẽ làm cạn kiệt luồng xử lý của các dịch vụ trước đó

* **Cơ chế Cascading Failure (Lỗi dây chuyền):** Nếu Service A gọi đồng bộ B, B gọi đồng bộ C; khi C bị nghẽn mạng hoặc phản hồi chậm, các luồng xử lý (threads) tại B và A đều bị giữ lại để chờ đợi. Khi số lượng request tăng lên, toàn bộ Thread Pool của máy chủ sẽ cạn kiệt, khiến toàn bộ ứng dụng ngừng phục vụ.
* **Tình huống giả định tại TechShop:**
  * Giả định Inventory gọi thêm một dịch vụ kiểm tra vị trí kho vận của đối tác bên ngoài.
  * Dịch vụ đối tác bị chậm → Inventory bị chậm → Order bị nghẽn luồng chờ đợi → Khách hàng không thể tạo đơn hàng.

```
[Khách hàng] ──> [Order Module] ──(Sync)──> [Inventory Module] ──(Sync)──> [Dịch vụ kho vận đối tác]
                 (Bị giữ luồng)             (Bị giữ luồng)                 (Bị chậm hoặc treo mạng!)
                       |
                       v
        TOÀN BỘ ỨNG DỤNG BỊ TẮC NGHẼN VÀ NGỪNG PHỤC VỤ REQUEST MỚI!
```

* **Nguyên tắc thiết kế:** Mọi bước xử lý không cần phản hồi ngay lập tức phải được chuyển sang **giao tiếp bất đồng bộ**.

<!-- Speaker Notes:
Nguy cơ lớn nhất của giao tiếp đồng bộ nhiều tầng là lỗi dây chuyền (Cascading Failure).
Nếu một dịch vụ phía sau bị chậm, tất cả các dịch vụ phía trước phải giữ luồng để chờ đợi, dẫn đến hiện tượng cạn kiệt Thread Pool.
Giả định nếu Inventory gọi thêm một dịch vụ đối tác vận chuyển bên ngoài mà dịch vụ đó bị nghẽn, toàn bộ luồng tạo đơn hàng tại Order sẽ bị tắc nghẽn theo.
Vì vậy, nguyên tắc thiết kế quan trọng là: bất kỳ tác vụ nào không cần kết quả ngay lập tức đều phải chuyển sang giao tiếp bất đồng bộ qua Message Queue.
-->

---

<!-- SLIDE 18: GIAO TIẾP BẤT ĐỒNG BỘ VÀ MESSAGE QUEUE -->
# Giao Tiếp Bất Đồng Bộ: Giải Phóng Luồng Xử Lý Bằng Hàng Đợi Thông Điệp <span class="tag-theory">[LÝ THUYẾT]</span>
### Tách rời sự phụ thuộc về thời gian giữa bên gửi và bên nhận trong chu trình đặt hàng

* **Khái niệm Message Queue:** Hệ thống trung gian nhận thông điệp từ bên gửi (Producer/Publisher) và lưu tạm thời để bên nhận (Consumer/Subscriber) lấy ra xử lý theo tốc độ riêng của mình.
* **Nguyên lý phi liên kết (Decoupling):** Producer và Consumer không cần chạy cùng một lúc, không cần biết vị trí hay chi tiết cài đặt của nhau.
* **Lợi ích thực tế:**
  * **Phản hồi nhanh cho người dùng:** Khách nhận kết quả tạo đơn thành công ngay sau khi giữ hàng và lưu đơn, không phải đợi gửi thông báo.
  * **Giảm phụ thuộc chặt (Loose Coupling):** Các tác vụ xử lý sau như gửi thông báo xác nhận, cập nhật số liệu thống kê được tách biệt hoàn toàn.

```
[Order Module] ──(Phát thông điệp)──> [Message Queue] ──(Rút thông điệp)──> [Notification Module]
  (Producer)                            (Hàng đợi trung gian)                    (Consumer)
```

<!-- Speaker Notes:
Để giải phóng luồng xử lý, chúng ta sử dụng giao tiếp bất đồng bộ qua Message Queue.
Message Queue đóng vai trò là một bộ đệm trung gian nhận thông điệp từ Producer và lưu trữ tạm thời cho Consumer lấy ra xử lý.
Hai bên không cần hoạt động cùng lúc và không phụ thuộc chặt vào nhau.
Lợi ích lớn nhất là khách hàng của TechShop nhận phản hồi thành công ngay lập tức; việc gửi email xác nhận hay cập nhật thống kê diễn ra ngầm phía sau mà không bắt khách phải đứng chờ.
Hệ thống Message Queue chuẩn mực được sử dụng phổ biến nhất hiện nay là RabbitMQ.
-->

---

<!-- SLIDE 19: CÁC KHÁI NIỆM CƠ BẢN CỦA RABBITMQ -->
# RabbitMQ: Năm Thành Phần Cốt Lõi Và Ba Loại Exchange Định Tuyến Thông Điệp <span class="tag-theory">[LÝ THUYẾT]</span>
### Cấu trúc điều phối thông điệp theo tiêu chuẩn công nghiệp AMQP 0-9-1

```
                  +-------------------------------------------------+
                  |                 RABBITMQ BROKER                 |
+------------+    |    +------------+        Binding Rules          |
|  PRODUCER  | ──Publish──> |  EXCHANGE  | ────────────────────+     |
+------------+    |    +------------+                     |     |
 (Order Svc)      |          |                            v     |
                  |          |                     +------------+    Consume     +------------+
                  |          +───────────────────> |   QUEUE    | ─────────────> |  CONSUMER  |
                  |                                +------------+  (Manual ACK)  +------------+
                  +-------------------------------------------------+             (Worker)
```

* **5 Thành phần cốt lõi:**
  * **Producer:** Ứng dụng tạo và gửi message.
  * **Exchange:** Tiếp nhận message từ Producer và định tuyến vào Queue.
  * **Queue:** Nơi lưu trữ message chờ Consumer xử lý.
  * **Binding:** Cấu hình liên kết giữa Exchange và Queue.
  * **Consumer:** Ứng dụng nhận message từ Queue để thực thi nghiệp vụ.
* **3 Loại Exchange:**
  * **Direct Exchange:** Định tuyến khớp chính xác Routing Key.
  * **Topic Exchange:** Định tuyến khớp mẫu Routing Key (dùng ký tự đại diện `*` và `#`).
  * **Fanout Exchange:** Định tuyến gửi tới **tất cả các Queue** đã bind vào Exchange (bỏ qua routing key).

<!-- Speaker Notes:
RabbitMQ hoạt động theo chuẩn AMQP 0-9-1 với 5 thành phần chính.
Producer gửi thông điệp vào Exchange. Exchange dựa trên cấu hình Binding để định tuyến thông điệp vào các Queue. Consumer lắng nghe Queue để nhận thông điệp và xử lý.
Có 3 loại Exchange phổ biến:
Direct Exchange khớp chính xác routing key; Topic Exchange khớp theo mẫu ký tự đại diện; và Fanout Exchange chuyển tiếp thông điệp tới tất cả các Queue đang kết nối.
Trong hệ thống của TechShop, khi đơn hàng được tạo, chúng ta cần phát tán sự kiện cho nhiều module cùng lúc, vì vậy Fanout Exchange là lựa chọn tối ưu.
-->

---

<!-- SLIDE 20: PUB/SUB VỚI FANOUT TRONG TECHSHOP -->
# Mô Hình Publish/Subscribe: Sự Kiện OrderCreated Được Phân Phối Tới Nhiều Queue <span class="tag-theory">[LÝ THUYẾT]</span>
### Order phát sự kiện một lần duy nhất — Các module tự nhận và xử lý song song, độc lập

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
      | Queue:            |                       | Queue:            |
      | 'inventory.order- |                       | 'notification.    |
      |  created'         |                       |  order-created'   |
      +-------------------+                       +-------------------+
                |                                           |
                v                                           v
      [Inventory Consumer]                        [Notification Consumer]
      (Xác nhận trừ kho:                          (Ghi log thông báo và
       Reserved -> Sold)                           gửi email mô phỏng)
```

* **Quy trình hoạt động:** Module Order phát một sự kiện `OrderCreated` lên exchange `order.events`. Exchange tự động sao chép message vào hai queue độc lập. Module Inventory và Notification tự nhận message và xử lý song song; Order không phải chờ bất kỳ consumer nào.
* *Ghi chú mở rộng:* Đề bài có nêu thêm Promotion (cập nhật lượt dùng mã) như ví dụ mở rộng; hệ thống demo hiện tại tập trung triển khai 2 consumer chính là Inventory và Notification.

<!-- Speaker Notes:
Mô hình Publish/Subscribe qua Fanout Exchange được TechShop áp dụng rất rõ nét.
Khi đơn hàng được xác nhận, Order chỉ phát một sự kiện OrderCreated duy nhất lên Fanout Exchange mang tên order.events.
Exchange sẽ nhân bản thông điệp và đưa vào hai queue riêng biệt: inventory.order-created và notification.order-created.
Inventory Consumer nhận tin để cập nhật trạng thái kho từ hàng giữ sang đã bán.
Notification Consumer nhận tin để ghi log và gửi thông báo.
Hai bên xử lý hoàn toàn song song và độc lập.
Tiếp theo, chúng ta cùng xem bản đồ giao tiếp tổng thể của toàn bộ luồng đặt hàng.
-->

---

<!-- SLIDE 21: BẢN ĐỒ GIAO TIẾP CỦA TECHSHOP -->
# Bản Đồ Giao Tiếp TechShop: Kết Hợp Cả Đồng Bộ Và Bất Đồng Bộ Trong Cùng Một Luồng <span class="tag-theory">[LÝ THUYẾT]</span>
### Nguyên tắc: Cần kết quả ngay thì Đồng bộ, chỉ báo sự kiện thì Bất đồng bộ

```
[Khách hàng]
     │
     │ 1. POST /api/orders (Đồng bộ)
     ▼
[Module ORDER] ────── 2. ReserveStock (Đồng bộ) ──────> [Module INVENTORY]
     │                                                        │
     │ 3. Lưu đơn vào schema 'order' (Confirmed)              │ (Tạm giữ tồn kho:
     │                                                        │  Available → Reserved)
     │ ┄ ┄ 4. Publish 'OrderCreated' (Bất đồng bộ) ┄ ┄ > [RABBITMQ BROKER]
     │                                                            │
     │ 5. Phản hồi khách: HTTP 201 Created                        ├────────────┐
     ▼                                                            │            │
Khách nhận kết quả đặt hàng thành công (< 50ms)                   ▼            ▼
                                                            [Queue: Kho]  [Queue: Notif]
                                                                  │             │
                                                                  ▼             ▼
                                                             (Chuyển bán)  (Gửi Email)
```

* **Quy ước đường truyền:** Mũi tên liền nét (`──>`) là giao tiếp đồng bộ; mũi tên đứt nét (`┄ ┄>`) là giao tiếp bất đồng bộ.

* **Bốn bước trong luồng đặt hàng:**
  1. *Giữ hàng (Đồng bộ):* Order gọi Inventory kiểm tra và giữ hàng (available → reserved).
  2. *Lưu đơn:* Order lưu đơn hàng trạng thái Confirmed vào CSDL.
  3. *Phát sự kiện (Bất đồng bộ):* Order phát `OrderCreatedEvent` lên RabbitMQ và phản hồi HTTP 201 cho khách.
  4. *Xử lý ngầm:* Inventory chuyển hàng từ reserved sang sold; Notification ghi bản ghi thông báo.

<!-- Speaker Notes:
Bản đồ giao tiếp của TechShop thể hiện rõ nét nguyên tắc: cần kết quả ngay thì dùng đồng bộ, chỉ cần thông báo thì dùng bất đồng bộ.
Mũi tên liền thể hiện lời gọi đồng bộ: Order gọi sang Inventory để giữ hàng, vì bắt buộc phải biết còn hàng thì mới cho phép tạo đơn.
Mũi tên đứt thể hiện giao tiếp bất đồng bộ: sau khi lưu đơn thành công, Order phát sự kiện OrderCreated lên RabbitMQ và trả về kết quả 201 cho khách hàng ngay lập tức.
Việc chuyển hàng giữ thành đã bán và việc gửi email thông báo diễn ra ngầm phía sau.
Nhưng liệu giao tiếp bất đồng bộ có bị mất thông điệp khi gặp sự cố hay không?
-->

---

<!-- SLIDE 22: ĐỘ TIN CẬY TRONG GIAO TIẾP BẤT ĐỒNG BỘ -->
# Cơ Chế Đảm Bảo Tính Tin Cậy Của Message Queue Trong Hệ Thống Phân Tán <span class="tag-theory">[LÝ THUYẾT]</span>
### Bất đồng bộ không đồng nghĩa với mất tin — Các cơ chế bảo vệ thông điệp được cấu hình

* **Các cơ chế kỹ thuật của RabbitMQ trong demo:**
  * **Queue Durable (`durable = true`):** Khai báo hàng đợi bền vững trên broker.
  * **Message Persistent (`delivery_mode = 2`):** Thông điệp được đánh dấu lưu trữ bền vững.
  * **Publisher Confirms:** Phía gửi nhận tín hiệu xác nhận từ broker mới hoàn tất gửi.
  * **Consumer Manual ACK (`autoAck = false`):** Consumer xử lý xong nghiệp vụ và lưu CSDL thành công mới gửi lệnh `basicAck` để xóa message khỏi queue.

<div class="grid-2">
<div class="box-inv">

### Đã Chứng Minh Trong Demo
* Tắt Consumer Notification → Message mới nằm chờ an toàn trong Queue (`Ready = 1`).
* Bật lại Consumer → Message được lấy ra xử lý, lưu log và hàng đợi trở về `Ready = 0`.

</div>
<div class="box-other">

### Giới Hạn Cần Minh Bạch
* **Chưa có Transactional Outbox:** Nếu ứng dụng sập ngay sau khi lưu đơn vào CSDL nhưng chưa kịp gửi sang RabbitMQ thì sự kiện có thể bị rớt.
* Không dùng các khẳng định tuyệt đối khi chưa có kiểm chứng thực tế.

</div>
</div>

<!-- Speaker Notes:
Nhiều người e ngại giao tiếp bất đồng bộ dễ làm mất dữ liệu.
Trên thực tế, RabbitMQ cung cấp nhiều cơ chế bảo vệ: Queue durable, Message persistent, Publisher Confirms và Consumer Manual ACK.
Trong demo hôm nay, nhóm đã kiểm chứng thực tế: khi tắt Consumer, thông điệp vẫn nằm chờ trong queue với trạng thái Ready, và khi bật lại thì consumer lập tức xử lý và gửi ACK.
Tuy nhiên, để minh bạch kỹ thuật: hệ thống demo chưa triển khai Transactional Outbox, nên nếu máy chủ sập giữa lúc lưu CSDL và gửi message thì sự kiện vẫn có rủi ro bị rớt. Chúng ta không dùng các từ cam kết tuyệt đối.
Bây giờ, chúng ta cùng chuyển sang Phần 4: Trình diễn 4 kịch bản demo thực tế.
-->

---

<!-- SLIDE 23: DEMO 1 - CẤU TRÚC MODULAR MONOLITH -->
# DEMO 1: Cấu Trúc Modular Monolith Và Ba Schema CSDL PostgreSQL Độc Lập <span class="tag-demo">[DEMO]</span>
### Kiểm chứng ranh giới mã nguồn, DbContext internal và ba schema trong cùng một CSDL

* **Thao tác thực hiện:**
  * Mở cây thư mục mã nguồn kiểm tra các module: `Order`, `Inventory`, `Inventory.Contracts`, `Notification`, `Shared`.
  * Truy vấn danh sách schema trong PostgreSQL bằng `psql`:
    ```sql
    SELECT schema_name FROM information_schema.schemata 
    WHERE schema_name IN ('order', 'inventory', 'notification');
    ```
  * Chạy test kiến trúc tự động (`Test4`, `Test5`) để kiểm chứng ranh giới.
* **Điều cần quan sát:**
  * Thư mục các module tách biệt; `Order` chỉ tham chiếu `Inventory.Contracts`.
  * Kết quả truy vấn hiển thị đủ 3 schema: `inventory`, `order`, `notification`.
  * DbContext của cả 3 module đều là `internal`; test kiến trúc NetArchTest đạt kết quả PASS.

```
[chèn ảnh chụp: cây thư mục dự án demo]
[chèn ảnh chụp: kết quả truy vấn 3 schema trong PostgreSQL]
```

<!-- Speaker Notes:
Bắt đầu phần demo thực tế với Demo 1: Cấu trúc Modular Monolith.
Trên cây thư mục dự án, các module Order, Inventory và Notification được phân chia thành các project độc lập.
Module Order hoàn toàn không tham chiếu tới project Inventory nội bộ mà chỉ tham chiếu tới project Inventory.Contracts.
Trong cơ sở dữ liệu PostgreSQL, truy vấn schemata trả về đủ 3 schema riêng biệt: order, inventory và notification.
Toàn bộ DbContext đều được đánh dấu internal, và bài test kiến trúc NetArchTest đã chạy thành công xác nhận không có bất kỳ phụ thuộc trái quy tắc nào.
-->

---

<!-- SLIDE 24: DEMO 2 - GỌI ĐỒNG BỘ ĐỦ HÀNG VÀ HẾT HÀNG -->
# DEMO 2: Gọi Đồng Bộ Kiểm Tra Tồn Kho — Đủ Hàng (201) Và Hết Hàng (409) <span class="tag-demo">[DEMO]</span>
### Kiểm chứng lời gọi kiểm tra và giữ hàng — Chuyển đổi mode Http không làm đổi OrderService

* **Thao tác thực hiện:**
  * Gửi request đặt mua 2 chiếc "Laptop Gaming Pro 16" (sản phẩm còn 5 chiếc trong kho).
  * Gửi request đặt mua 1 chiếc "Chuột Không Dây RGB" (sản phẩm có tồn kho bằng 0).
  * Quan sát log xử lý giữa Order và Inventory.
  * Đổi cấu hình `Inventory:Mode` từ `InProcess` sang `Http` trong `appsettings.json` và chạy lại kịch bản.
* **Điều cần quan sát:**
  * Tình huống đủ hàng: Nhận mã **HTTP 201 Created**, đơn hàng trạng thái `Confirmed`.
  * Tình huống hết hàng: Nhận mã **HTTP 409 Conflict**, đơn hàng trạng thái `Rejected`.
  * Khi chuyển sang mode `Http`, kết quả nghiệp vụ hoàn toàn giống nhau; log hiển thị lời gọi qua REST endpoint `/internal/inventory/reserve`.
* **Ý nghĩa kiến trúc:** `OrderService` giữ nguyên 100% mã nguồn khi đổi cách gọi; chế độ REST hiện gọi loopback trong cùng tiến trình.

```
[chèn ảnh chụp: HTTP response 201 Created và 409 Conflict]
[chèn ảnh chụp: Log terminal hiển thị [ORDER] và [INVENTORY-HTTP]]
```

<!-- Speaker Notes:
Demo 2 kiểm chứng giao tiếp đồng bộ kiểm tra kho.
Ở tình huống thứ nhất, khi đặt mua Laptop Gaming Pro còn hàng, hệ thống giữ hàng thành công và trả về HTTP 201 Created.
Ở tình huống thứ hai, khi đặt mua Chuột Không Dây RGB có số lượng tồn bằng 0, hệ thống từ chối giữ hàng và trả về HTTP 409 Conflict.
Đặc biệt, khi chuyển cấu hình Inventory:Mode sang Http, Order gọi sang Inventory qua REST loopback nhưng OrderService hoàn toàn không phải thay đổi một dòng code nào.
Điều này chứng minh tính hữu hiệu của việc sử dụng Public Interface từ đầu.
Tiếp theo, chúng ta cùng quan sát giao tiếp bất đồng bộ với RabbitMQ.
-->

---

<!-- SLIDE 25: DEMO 3 - RABBITMQ VÀ CONSUMER OFFLINE -->
# DEMO 3: RabbitMQ Thật — Tắt Consumer, Message Vẫn Nằm Chờ Trong Hàng Đợi <span class="tag-demo">[DEMO]</span>
### Kiểm chứng tính độc lập của bên nhận và cơ chế lưu tạm message khi consumer gián đoạn

* **Thao tác thực hiện:**
  * Mở giao diện RabbitMQ Management UI tại cổng `15672`.
  * Tạo đơn hàng đầu tiên: Quan sát cả 2 queue `inventory.order-created` và `notification.order-created` nhận message.
  * Gọi API `POST /demo/consumers/notification/stop` để dừng Consumer Notification.
  * Tạo thêm đơn hàng thứ hai trong lúc Consumer Notification đang dừng.
  * Quan sát trạng thái hàng đợi trên RabbitMQ UI.
  * Gọi API `POST /demo/consumers/notification/start` để bật lại Consumer Notification.
* **Điều cần quan sát:**
  * Khi Consumer Notification dừng, message mới nằm chờ trong hàng đợi: cột **Ready = 1**.
  * Khi bật lại, Ready giảm về **0**; bảng `notification.notification_logs` xuất hiện bản ghi thông báo mới; consumer gửi Manual ACK.
  * Trong suốt quá trình, Inventory vẫn nhận và xác nhận trừ kho bình thường, không bị ảnh hưởng.

```
[chèn ảnh chụp: RabbitMQ UI khi Notification Consumer đang tắt - Ready = 1]
[chèn ảnh chụp: RabbitMQ UI sau khi bật lại - Ready = 0 và log xử lý email]
```

<!-- Speaker Notes:
Demo 3 là minh chứng thực tế về giao tiếp bất đồng bộ qua RabbitMQ thật.
Đầu tiên, chúng ta thấy trên RabbitMQ UI cả hai queue đều nhận message khi có đơn hàng mới.
Sau đó, tôi gọi API dừng Notification Consumer và tạo thêm một đơn hàng nữa.
Trên RabbitMQ UI, cột Ready của queue notification tăng lên 1, chứng minh message nằm chờ an toàn trong hàng đợi.
Khi tôi gọi API bật lại consumer, message ngay lập tức được lấy ra xử lý, email mô phỏng được ghi vào cơ sở dữ liệu, và cột Ready trở về 0.
Quá trình này hoàn toàn không làm gián đoạn việc trừ kho của Inventory.
-->

---

<!-- SLIDE 26: DEMO 4 - TRANH CHẤP SẢN PHẨM CUỐI -->
# DEMO 4: Hai Đơn Cùng Mua Sản Phẩm Cuối Cùng — Chống Bán Vượt Tồn Bằng Atomic Update <span class="tag-demo">[DEMO]</span>
### Kiểm chứng tính nhất quán dữ liệu khi xảy ra tranh chấp đặt hàng đồng thời

* **Thao tác thực hiện:**
  * Sản phẩm "Laptop Gaming Cuối Cùng" có số lượng tồn khả dụng bằng **đúng 1 chiếc**.
  * Chạy script kiểm thử tranh chấp `demo/race.ps1` gửi đồng thời 2 request đặt mua song song.
* **Điều cần quan sát:**
  * Kết quả thực tế: Đúng 1 request nhận **HTTP 201 Created** (thành công) và đúng 1 request nhận **HTTP 409 Conflict** (bị từ chối).
  * Tồn kho sau tranh chấp: Khả dụng = `0`, Đã bán = `1`; tuyệt đối không bị bán âm kho.
* **Quy trình 3 bước xử lý kho:**
  1. *Kiểm tra tồn kho:* Xác định số lượng khả dụng.
  2. *Giữ hàng nguyên tử (Reserve):* Thực thi lệnh SQL `UPDATE ... WHERE available_quantity >= @qty`.
  3. *Xác nhận đã bán (Sold):* Chuyển từ reserved sang sold khi nhận sự kiện qua RabbitMQ (hoặc nhả hàng nếu đơn lỗi).
* **Kết quả test tự động:** Đã chạy bộ 5 test xUnit thật trong `ModularMonolith.Tests` → **Passed 5/5 tests trong 4 giây**.

```
[chèn ảnh chụp: Kết quả thực thi script race.ps1 hiển thị 1x201 và 1x409]
[chèn ảnh chụp: Kết quả chạy 5/5 tests xUnit thành công trong terminal]
```

<!-- Speaker Notes:
Demo 4 giải quyết bài toán đặt ra ở đầu bài: hai khách hàng cùng tranh mua chiếc laptop cuối cùng.
Script race.ps1 bắn đồng thời hai request đặt mua vào hệ thống.
Nhờ câu lệnh SQL UPDATE nguyên tử có điều kiện available_quantity >= qty, CSDL khóa dòng và xử lý chính xác: đúng 1 khách nhận mã 201 Created và 1 khách nhận mã 409 Conflict.
Số lượng tồn khả dụng trở về 0 và số lượng đã bán là 1, không bao giờ bị bán âm.
Bộ 5 bài test tự động xUnit của dự án cũng đã chạy thực tế và vượt qua 100%.
Tiếp theo, chúng ta bước vào Phần 5: Hướng mở rộng và tổng kết.
-->

---

<!-- SLIDE 27: HƯỚNG MỞ RỘNG: TÁCH INVENTORY THÀNH MICROSERVICE -->
# Hướng Mở Rộng: Các Bước Cần Thay Đổi Nếu Tách Inventory Thành Microservice <span class="tag-ext">[MỞ RỘNG]</span>
### Đánh giá các thành phần thay đổi và thành phần được giữ nguyên — Tình huống phát triển tiếp theo

<div class="grid-2">
<div class="box-inv">

### Các Thành Phần Cần Thay Đổi
* Schema `inventory` được chuyển sang máy chủ cơ sở dữ liệu vật lý độc lập.
* Cấu hình `HttpInventoryService` trong Order trỏ tới địa chỉ mạng của service mới.
* Bổ sung Dockerfile độc lập cho module Inventory.
* Tạo pipeline CI/CD riêng biệt kích hoạt theo đường dẫn thư mục `src/Modules/Inventory/**`.

</div>
<div class="box-order">

### Các Thành Phần Được Giữ Nguyên
* Mã nguồn `OrderService` **giữ nguyên 100%** không đổi một dòng code.
* Logic nghiệp vụ giữ hàng, nhả hàng và trừ kho trong `Inventory.Domain` giữ nguyên 100%.
* Contract `IInventoryService` và các DTO giữ nguyên.
* Cách Consumer Inventory nhận sự kiện `OrderCreated` qua RabbitMQ giữ nguyên.

</div>
</div>

<div class="warning-box" style="text-align: center; font-weight: bold;">
LƯU Ý MINH BẠCH: "Đây là hướng phát triển mở rộng; dự án demo hiện tại chưa tách thành microservice thật."
</div>

<!-- Speaker Notes:
Slide 27 trình bày hướng mở rộng nếu sau này TechShop cần tách Inventory thành microservice riêng để chịu tải mùa sale.
Bảng so sánh chỉ rõ: những gì cần thay đổi là tách CSDL vật lý, đổi URL trong HttpInventoryService, thêm Dockerfile và pipeline CI/CD riêng.
Ngược lại, toàn bộ mã nguồn nghiệp vụ của OrderService, logic trừ kho của Inventory và interface IInventoryService đều được giữ nguyên 100%.
Xin lưu ý rõ ràng: đây là hướng phát triển về mặt kiến trúc, dự án demo hiện tại chưa tách thành microservice thật.
Bây giờ, chúng ta cùng đến với phần tổng kết toàn bộ chuyên đề.
-->

---

<!-- SLIDE 28: TỔNG KẾT VÀ GIỚI HẠN CỦA DEMO -->
# Tổng Kết Chuyên Đề Và Minh Bạch Các Giới Hạn Kỹ Thuật Hiện Tại Của Bản Demo <span class="tag-theory">[TỔNG KẾT]</span>
### Ba bài học cốt lõi cho kỹ sư phần mềm và các điểm giới hạn cần hoàn thiện

* **Ba kết luận kiến trúc cốt lõi:**
  1. *Không có kiến trúc luôn đúng:* Lựa chọn kiến trúc phải căn cứ theo quy mô đội ngũ, năng lực vận hành và giai đoạn sản phẩm thay vì áp dụng theo trào lưu.
  2. *Ranh giới module là phần quan trọng nhất:* Sử dụng Bộ 4 câu hỏi định lượng để phân rã nghiệp vụ và giữ kỷ luật cô lập CSDL nghiêm ngặt.
  3. *Kết hợp linh hoạt cơ chế giao tiếp:* Cần kết quả ngay để ra quyết định thì dùng đồng bộ; chỉ cần thông báo sự việc đã xảy ra thì dùng bất đồng bộ.
* **Bốn (4) giới hạn kỹ thuật của bản demo hiện tại:**
  * Gửi Email/SMS là **mô phỏng (Simulation)** ghi log và CSDL, chưa tích hợp dịch vụ gửi thư thật.
  * Chế độ Http REST đang gọi loopback trong cùng tiến trình, chưa chạy trên hai máy chủ mạng riêng biệt.
  * Chưa triển khai Transactional Outbox; nếu ứng dụng sập ngay sau khi lưu đơn thì sự kiện có thể chưa được gửi sang RabbitMQ.
  * Cơ sở dữ liệu chạy trên 1 PostgreSQL server, cô lập ở cấp độ schema, chưa phân quyền user database riêng biệt.

<!-- Speaker Notes:
Để khép lại bài thuyết trình, nhóm xin đúc kết ba bài học lớn:
Thứ nhất, không có kiến trúc nào là vạn năng; Modular Monolith là điểm khởi đầu phù hợp cho phần lớn ứng dụng.
Thứ hai, ranh giới nghiệp vụ và kỷ luật cô lập CSDL là yếu tố quyết định sự thành bại của kiến trúc.
Thứ ba, cần kết hợp nhuần nhuyễn giữa gọi đồng bộ để giữ hàng và bất đồng bộ qua RabbitMQ để phát sự kiện.
Đồng thời, nhóm xin minh bạch bốn giới hạn hiện tại của bản demo: email là mô phỏng, REST gọi loopback, chưa có Outbox pattern, và CSDL dùng chung một server PostgreSQL phân tách theo schema.
Cảm ơn quý thầy cô và các bạn đã chú ý theo dõi. Tiếp theo là các phụ lục tra cứu khi thảo luận.
-->

---

<!-- SLIDE 29: PHỤ LỤC A - MA TRẬN 10 NGHIỆP VỤ -->
# PHỤ LỤC A: Ma Trận Đánh Giá Toàn Diện Mười Nghiệp Vụ E-Commerce × Bộ 4 Câu Hỏi <span class="tag-app">[PHỤ LỤC]</span>
### Tài liệu tra cứu chi tiết phục vụ phiên thảo luận và phản biện chuyên sâu

| STT | Nghiệp vụ | 1. Ngôn ngữ riêng | 2. Trách nhiệm tổ chức | 3. Độc lập CSDL (No-JOIN) | 4. Tốc độ đổi | Quyết định đề xuất |
|:---:|---|---|---|---|:---:|:---:|
| **1** | **Catalog** | Specs, SEO, DisplayPrice | Merchandising | Độc lập cao: Lưu thông tin mô tả, ảnh | Thấp - TB | Tách riêng (Lõi) |
| **2** | **Cart** | CartItem, GuestSession | Growth & UX Team | Độc lập cao: Lưu tạm thời, xóa theo phiên | Trung bình | Tách riêng (Lõi) |
| **3** | **Order** | SalesOrder, OrderLineItem | Vận hành Đơn hàng | Độc lập cao: Lưu **Snapshot** sản phẩm | Thấp - TB | Tách riêng (Lõi) |
| **4** | **Payment** | Transaction, Settlement | Tài chính - Kế toán | Tuyệt đối: Lưu log đối soát, mã giao dịch | Thấp | Tách riêng (Lõi) |
| **5** | **Inventory**| SKU, StockLevel, ATP | Quản lý Kho vận | Độc lập cao: Quản lý số lượng theo SKU | Thấp | Tách riêng (Lõi) |
| **6** | **Shipping** | Waybill, Consignment | Logistics & 3PL | Độc lập cao: Lưu địa chỉ giao, cân nặng | Trung bình | Tách riêng (Lõi) |
| **7** | **Identity** | UserAccount, Role, JWT | An ninh mạng & IT | Tuyệt đối: Quản lý tài khoản và hash | Rất thấp | Tách riêng (Lõi) |
| **8** | **Notification**| Template, Kênh gửi tin | Chăm sóc Khách hàng| Tuyệt đối: Nhận payload từ hàng đợi | Trung bình | Tách riêng (Lõi) |
| **9** | **Promotion** | Voucher, Quota, FlashSale| Marketing | Độc lập cao: Tính toán giảm trừ qua DTO | Rất cao | Tách riêng (Mở rộng) |
| **10**| **Review** | Rating, VerifiedBadge | Quản trị Chất lượng | Độc lập cao: Lưu đánh giá theo ProductId | Thấp | Tách riêng (Mở rộng) |

<!-- Speaker Notes:
Phụ lục A cung cấp bảng ma trận đầy đủ cho cả 10 nghiệp vụ thương mại điện tử theo Bộ 4 câu hỏi vàng.
Bảng này dùng để tra cứu khi hội đồng chất vấn chi tiết về lý do phân rã của từng nghiệp vụ cụ thể trong hệ thống.
-->

---

<!-- SLIDE 30: PHỤ LỤC B - BẢNG THUẬT NGỮ KỸ THUẬT -->
# PHỤ LỤC B: Bảng Thuật Ngữ Kỹ Thuật Định Nghĩa Một Dòng <span class="tag-app">[PHỤ LỤC]</span>
### Định nghĩa chuẩn xác các khái niệm kỹ thuật cốt lõi trong kiến trúc hệ thống máy chủ

* **Monolithic Architecture:** Kiến trúc trong đó toàn bộ ứng dụng được đóng gói trong một codebase, chạy trong một tiến trình duy nhất và dùng chung một cơ sở dữ liệu.
* **Modular Monolithic:** Kiến trúc một tiến trình thực thi duy nhất nhưng mã nguồn được phân rã thành các module độc lập theo nghiệp vụ và cô lập CSDL.
* **Microservices Architecture:** Kiến trúc phân tán trong đó mỗi dịch vụ nghiệp vụ chạy độc lập, sở hữu cơ sở dữ liệu riêng và giao tiếp qua mạng.
* **Domain-Driven Design (DDD):** Phương pháp luận thiết kế phần mềm tập trung vào việc mô hình hóa hệ thống bám sát miền nghiệp vụ thực tế của doanh nghiệp.
* **Bounded Context:** Ranh giới logic xác định phạm vi mà một mô hình nghiệp vụ và ngôn ngữ chung (Ubiquitous Language) được áp dụng nhất quán.
* **Architectural Seam:** Đường cắt giao tiếp trừu tượng (như Public Interface) cho phép thay đổi cơ chế thực thi mà không sửa đổi mã nguồn nơi gọi.
* **Synchronous Communication:** Cơ chế giao tiếp dạng Request - Response trong đó tiến trình gọi phải chờ phản hồi từ tiến trình nhận trước khi tiếp tục.
* **Asynchronous Communication:** Cơ chế giao tiếp hướng sự kiện qua Message Queue trong đó tiến trình gửi phát thông điệp và giải phóng luồng ngay lập tức.
* **RabbitMQ Exchange / Queue / Binding:** Bộ định tuyến thông điệp AMQP (Exchange), hàng đợi lưu trữ thông điệp (Queue), và quy tắc ánh xạ định tuyến (Binding).
* **Cascading Failure:** Hiện tượng lỗi sụp đổ dây chuyền trong hệ thống phân tán khi sự cố hoặc tắc nghẽn ở một dịch vụ kéo theo sự sụp đổ của các dịch vụ khác.

<!-- Speaker Notes:
Phụ lục B tổng hợp định nghĩa kỹ thuật ngắn gọn, chính xác một dòng cho 10 thuật ngữ cốt lõi của môn học.
Bảng này hỗ trợ tra cứu nhanh trong phần thảo luận học thuật với giảng viên.
-->

---

<!-- SLIDE 31: PHỤ LỤC C - TÀI LIỆU THAM KHẢO -->
# PHỤ LỤC C: Danh Mục Tài Liệu Tham Khảo Chính Thống Của Môn Học <span class="tag-app">[PHỤ LỤC]</span>
### Các nguồn tài liệu học thuật và kỹ thuật chính thức làm căn cứ nghiên cứu

* **Tài liệu học phần chính thức:**
  * Tài liệu học phần: *Buổi 12 Modular Monolithic – Tư duy thiết kế và con đường tiến hóa lên Microservice* (Bộ môn Kỹ thuật Phần mềm).
  * Hướng dẫn phân công: *Kế Hoạch Phân Công Nghiên Cứu Lý Thuyết Buổi 12* và *Phan_cong_ly_thuyet_Buoi_12*.
* **Tài liệu kỹ thuật chính thức của công nghệ:**
  * **RabbitMQ Documentation:** rabbitmq.com/tutorials (AMQP 0-9-1 Concepts, Exchanges, Queues và Reliability Guide).
  * **gRPC Documentation & Protocol Buffers:** grpc.io/docs và protobuf.dev (HTTP/2 transport, proto3 specification).
  * **Npgsql & Entity Framework Core:** npgsql.org/efcore (PostgreSQL schema mapping và Concurrency token).
* **Bài viết và tài liệu kiến trúc:**
  * **Martin Fowler:** Bài viết *"MonolithFirst"* (2015, martinfowler.com/bliki/MonolithFirst.html).
  * **Shopify Engineering:** Kinh nghiệm thực tế tái cấu trúc và vận hành Modular Monolith quy mô lớn.
  * **Eric Evans:** Sách chuyên khảo *"Domain-Driven Design: Tackling Complexity in the Heart of Software"* (Bounded Context).

<!-- Speaker Notes:
Phụ lục C liệt kê đầy đủ danh mục tài liệu tham khảo chính thống: giáo trình môn học, tài liệu chính thức của RabbitMQ, gRPC, bài viết MonolithFirst của Martin Fowler và sách Domain-Driven Design của Eric Evans.
-->

---

<!-- SLIDE 32: PHỤ LỤC D - CÂU HỎI PHẢN BIỆN CHUYÊN SÂU -->
# PHỤ LỤC D: Tóm Lược Các Câu Hỏi Phản Biện Chuyên Sâu Thường Gặp <span class="tag-app">[PHỤ LỤC]</span>
### Hướng dẫn trả lời chuẩn xác các câu hỏi trọng tâm mà Hội đồng thường đặt ra

<div class="grid-2">
<div class="box-other">

### 1. Câu hỏi về Kiến trúc
* **Có bắt buộc mọi hệ thống lớn phải lên Microservices không?**
  * *Trả lời:* Hoàn toàn không. Modular Monolith là kiến trúc đích hoàn chỉnh của nhiều hệ thống lớn. Microservices không phải đích đến bắt buộc.
* **Chi phí ẩn lớn nhất của Microservices là gì?**
  * *Trả lời:* Khoản thuế phân tán gồm chi phí hạ tầng máy chủ, độ trễ mạng, mất tính toàn vẹn ACID và chi phí đội ngũ DevOps chuyên sâu.

</div>
<div class="box-other">

### 2. Câu hỏi về Ranh giới & Kỹ thuật
* **Ranh giới giữa Cart và Order có tuyệt đối duy nhất không?**
  * *Trả lời:* Không có đáp án tuyệt đối; tách hay gộp đều chấp nhận được nếu giải thích được căn cứ theo Bộ 4 câu hỏi vàng.
* **Làm sao đảm bảo chống bán vượt tồn khi 2 khách cùng mua?**
  * *Trả lời:* Thực hiện câu lệnh SQL nguyên tử `UPDATE stocks SET available_quantity = available_quantity - @qty, reserved_quantity = reserved_quantity + @qty WHERE available_quantity >= @qty` (đã chứng minh bằng `race.ps1`).

</div>
</div>

<div class="highlight">
Chi tiết hướng dẫn trả lời đầy đủ 15 câu hỏi xem tại tệp: <code>ngan_hang_cau_hoi_phan_bien_buoi_12.md</code>.
</div>

<!-- Speaker Notes:
Phụ lục D tóm lược 4 câu hỏi phản biện cốt lõi mà hội đồng thường thử thách sinh viên: tính bắt buộc của Microservices, chi phí ẩn phân tán, sự linh hoạt của ranh giới Cart/Order, và cơ chế chống bán vượt tồn bằng câu lệnh SQL nguyên tử.
-->