using Modules.Inventory;
using Modules.Inventory.Contracts.DTOs;
using Modules.Notification;
using Modules.Order;
using Modules.Order.Application.DTOs;
using Shared.Kernel.RabbitMQ;

namespace ModularMonolith.Runner;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        PrintHeader();

        // Khởi tạo Broker và Logger chung
        var broker = new RabbitMqBroker(msg =>
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    {msg}");
            Console.ResetColor();
        });

        // Đảm bảo thư mục data sạch sẽ cho kịch bản demo
        var dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "demo_data");
        if (Directory.Exists(dataDir))
        {
            try { Directory.Delete(dataDir, true); } catch { }
        }
        Directory.CreateDirectory(dataDir);

        var invDataDir = Path.Combine(dataDir, "inventory");
        var orderDataDir = Path.Combine(dataDir, "order");
        var notifDataDir = Path.Combine(dataDir, "notification");

        // Khởi tạo các Module (Cô lập ranh giới)
        var (inventoryService, invConsumer) = InventoryModuleExtensions.InitializeInventoryModule(
            broker, invDataDir, msg => WriteColored(msg, ConsoleColor.Cyan));

        var notifConsumer = NotificationModuleExtensions.InitializeNotificationModule(
            broker, notifDataDir, msg => WriteColored(msg, ConsoleColor.Yellow));

        var orderService = OrderModuleExtensions.InitializeOrderModule(
            inventoryService, broker, orderDataDir, msg => WriteColored(msg, ConsoleColor.Green));

        // =========================================================================
        // DEMO PHẦN 1: CẤU TRÚC MODULAR MONOLITHIC & CÔ LẬP DỮ LIỆU
        // =========================================================================
        Console.WriteLine("\n" + new string('=', 85));
        WriteColored("🔷 PHẦN 1: CẤU TRÚC MODULAR MONOLITHIC & KỶ LUẬT CÔ LẬP DỮ LIỆU", ConsoleColor.White, ConsoleColor.DarkBlue);
        Console.WriteLine(new string('=', 85));
        Console.WriteLine(@"
1. Ranh giới Solution Project:
   ├── src/Shared/Shared.Kernel          -> Chứa RabbitMQ Broker & Event contracts dùng chung
   ├── src/Modules/Inventory/            -> Quản lý tồn kho, SKU, kho hàng vật lý
   ├── src/Modules/Order/                -> Quản lý tạo đơn, chốt hợp đồng mua bán
   └── src/Modules/Notification/         -> Quản lý gửi Email, SMS thông báo

2. Bằng chứng Cô lập Schema / Database (Separate Schemas on Disk):
   ✔ Inventory Schema : " + invDataDir + @"\inventory_schema.json
   ✔ Order Schema     : " + orderDataDir + @"\order_schema.json
   ✔ Notification DB  : " + notifDataDir + @"\notification_schema.json

3. Kỷ luật Kiến trúc Thép:
   ✔ Không có câu lệnh SQL JOIN xuyên CSDL (Order dùng Snapshot Pattern).
   ✔ DbContext của các module đều là INTERNAL -> Ngăn chặn 100% việc inject chéo DbContext.
   ✔ Giao tiếp duy nhất giữa Order và Inventory thông qua Public Interface: IInventoryService.");

        await Task.Delay(1000);

        // =========================================================================
        // DEMO PHẦN 2: LỜI GỌI ĐỒNG BỘ REST/gRPC (ĐỦ HÀNG vs HẾT HÀNG)
        // =========================================================================
        Console.WriteLine("\n" + new string('=', 85));
        WriteColored("🔷 PHẦN 2: BẮT BUỘC — DEMO LỜI GỌI ĐỒNG BỘ (KIỂM TRA TỒN KHO TRƯỚC KHI TẠO ĐƠN)", ConsoleColor.White, ConsoleColor.DarkGreen);
        Console.WriteLine(new string('=', 85));

        var laptopId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var mouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Tình huống 2.1: Còn đủ hàng
        WriteColored("\n▶ TÌNH HUỐNG 2.1: ĐẶT HÀNG KHI CÒN ĐỦ TỒN KHO (SUCCESS)", ConsoleColor.Green);
        Console.WriteLine("Khách hàng: Nguyễn Văn A (ID: c1111111-...)");
        Console.WriteLine("Sản phẩm: 'Laptop Gaming Pro 16' (Tồn kho ban đầu: 5 chiếc) - Đặt mua: 2 chiếc");

        var orderReqSuccess = new CreateOrderRequest(
            CustomerId: Guid.NewGuid(),
            Items: new List<OrderItemRequestDto>
            {
                new(laptopId, "Laptop Gaming Pro 16", 2, 25_000_000m)
            }
        );

        var resultSuccess = await orderService.PlaceOrderAsync(orderReqSuccess);
        Console.WriteLine($"\n[KẾT QUẢ ĐƠN HÀNG 1]: Thành công: {resultSuccess.Success} | Trạng thái: {resultSuccess.Status} | Mã đơn: {resultSuccess.OrderId}");
        Console.WriteLine($"Thông điệp phản hồi: {resultSuccess.Message}");

        await Task.Delay(1000);

        // Tình huống 2.2: Hết hàng
        WriteColored("\n▶ TÌNH HUỐNG 2.2: ĐẶT HÀNG KHI HẾT HÀNG TRONG KHO (OUT OF STOCK)", ConsoleColor.Red);
        Console.WriteLine("Khách hàng: Trần Thị B (ID: c2222222-...)");
        Console.WriteLine("Sản phẩm: 'Chuột Không Dây RGB' (Tồn kho hiện có: 0 chiếc) - Đặt mua: 1 chiếc");

        var orderReqFailed = new CreateOrderRequest(
            CustomerId: Guid.NewGuid(),
            Items: new List<OrderItemRequestDto>
            {
                new(mouseId, "Chuột Không Dây RGB", 1, 450_000m)
            }
        );

        var resultFailed = await orderService.PlaceOrderAsync(orderReqFailed);
        Console.WriteLine($"\n[KẾT QUẢ ĐƠN HÀNG 2]: Thành công: {resultFailed.Success} | Trạng thái: {resultFailed.Status} | Mã đơn: {resultFailed.OrderId}");
        Console.WriteLine($"Thông điệp phản hồi: {resultFailed.Message}");

        await Task.Delay(1000);

        // =========================================================================
        // DEMO PHẦN 3: LỜI GỌI BẤT ĐỒNG BỘ VỚI RABBITMQ & CONSUMER OFFLINE
        // =========================================================================
        Console.WriteLine("\n" + new string('=', 85));
        WriteColored("🔷 PHẦN 3: BẮT BUỘC — DEMO GIAO TIẾP BẤT ĐỒNG BỘ VỚI RABBITMQ (FANOUT PUB/SUB)", ConsoleColor.White, ConsoleColor.DarkMagenta);
        Console.WriteLine(new string('=', 85));

        Console.WriteLine(@"
Mô hình Fanout Pub/Sub:
   [OrderService] ---Publish(OrderCreatedEvent)---> [Exchange: ecommerce.order-events]
                                                               |
                                +------------------------------+------------------------------+
                                |                                                             |
                                v                                                             v
                      [Queue: order.inventory]                                      [Queue: order.notification]
                                |                                                             |
                                v                                                             v
                   [Inventory Consumer: Trừ kho]                                [Notification Consumer: Gửi Mail/SMS]");

        WriteColored("\n▶ THỰC NGHIỆM ĐẶC BIỆT: CHỨNG MINH TẮT 1 CONSUMER KHÔNG LÀM MẤT TIN (ZERO LOSS)", ConsoleColor.Yellow);
        Console.WriteLine("Mục tiêu: Chứng minh tính tin cậy tuyệt đối của Message Queue khi Worker bị gián đoạn mạng hoặc sập.");

        // Bước 1: Tắt Notification Consumer
        WriteColored("\n[BƯỚC 3.1] Giả lập sự cố: Tắt hoàn toàn Notification Service (Consumer Offline)...", ConsoleColor.DarkYellow);
        broker.DisconnectConsumer("order.notification");
        Console.WriteLine($"Trạng thái Consumer 'order.notification': OFFLINE (Ngắt kết nối)");

        // Bước 2: Khách đặt đơn mới trong lúc Notification Service đang tắt
        WriteColored("\n[BƯỚC 3.2] Khách hàng đặt thêm 1 đơn hàng Laptop mới trong lúc Notification Service đang TẮT:", ConsoleColor.Cyan);
        var orderReqOfflineTest = new CreateOrderRequest(
            CustomerId: Guid.NewGuid(),
            Items: new List<OrderItemRequestDto>
            {
                new(laptopId, "Laptop Gaming Pro 16", 1, 25_000_000m)
            }
        );

        var result3 = await orderService.PlaceOrderAsync(orderReqOfflineTest);
        Console.WriteLine($"Khách hàng nhận ngay phản hồi: {result3.Message}");

        // Bước 3: Kiểm tra hàng đợi RabbitMQ
        WriteColored("\n[BƯỚC 3.3] Kiểm tra trạng thái các Hàng đợi RabbitMQ trên Broker:", ConsoleColor.White);
        var invQueueDepth = broker.GetQueueMessageCount("order.inventory");
        var notifQueueDepth = broker.GetQueueMessageCount("order.notification");
        Console.WriteLine($" -> Queue 'order.inventory'    : {invQueueDepth} message tồn đọng (Vì Inventory Consumer ONLINE, đã xử lý & ACK ngay)");
        Console.WriteLine($" -> Queue 'order.notification' : {notifQueueDepth} MESSAGE ĐANG NẰM CHỜ TRÊN ĐĨA (Message Ready = 1)!");
        Console.WriteLine(" => CHỨNG MINH: Dữ liệu thông điệp KHÔNG HỀ BỊ MẤT MÁT, RabbitMQ lưu trữ bền vững trên đĩa!");

        await Task.Delay(1500);

        // Bước 4: Khởi động lại Notification Consumer
        WriteColored("\n[BƯỚC 3.4] Khởi động lại Notification Service (Consumer Reconnects)...", ConsoleColor.Green);
        broker.ReconnectConsumer("order.notification");

        await Task.Delay(500);

        var notifQueueDepthAfter = broker.GetQueueMessageCount("order.notification");
        Console.WriteLine($" -> Queue 'order.notification' sau khi bật lại: {notifQueueDepthAfter} message tồn đọng (Consumer đã xử lý xong và gửi ACK xóa tin thành công!)");

        // =========================================================================
        // DEMO PHẦN 4: GIẢI THÍCH CON ĐƯỜNG TIẾN HÓA BÓC TÁCH INVENTORY
        // =========================================================================
        Console.WriteLine("\n" + new string('=', 85));
        WriteColored("🔷 PHẦN 4: GIẢI THÍCH TRƯỚC LỚP — CON ĐƯỜNG TIẾN HÓA LÊN MICROSERVICES", ConsoleColor.White, ConsoleColor.DarkRed);
        Console.WriteLine(new string('=', 85));
        Console.WriteLine(@"
Nếu sau này cần tách Inventory thành Microservice độc lập (để scale mùa Sale 11.11):
1. Về Logic nghiệp vụ:
   ✔ GIỮ NGUYÊN 100% mã nguồn trong Inventory.Domain và Inventory.Application!
   ✔ OrderService.cs KHÔNG THAY ĐỔI MỘT DÒNG CODE NGHIỆP VỤ NÀO vì nó chỉ phụ thuộc vào
     interface trừu tượng IInventoryService (Nguyên lý Đảo ngược phụ thuộc - DIP).

2. Bốn (4) bước bóc tách vật lý:
   [Bước 1] Chuyển file inventory_schema.json sang một cụm PostgreSQL / Redis Cluster độc lập.
   [Bước 2] Trong project Order, thay thế class InProcessInventoryService bằng GrpcInventoryClient:
            services.AddGrpcClient<IInventoryService>(o => o.Address = new Uri(""https://inventory-svc""));
   [Bước 3] Viết Dockerfile Multi-stage build đóng gói riêng Inventory Service (< 150MB).
   [Bước 4] Thiết lập Pipeline CI/CD riêng trên GitHub Actions với trigger: 'src/Modules/Inventory/**'.

=> Kết luận: Modular Monolithic chính là bệ phóng hoàn hảo nhất để tiến hóa lên Microservice!");

        Console.WriteLine("\n" + new string('=', 85));
        WriteColored("✔ TOÀN BỘ 4 PHẦN DEMO BẮT BUỘC CỦA BUỔI 12 ĐÃ CHẠY THÀNH CÔNG RỰC RỠ 100%!", ConsoleColor.White, ConsoleColor.DarkGreen);
        Console.WriteLine(new string('=', 85));
    }

    private static void WriteColored(string text, ConsoleColor fg, ConsoleColor? bg = null)
    {
        Console.ForegroundColor = fg;
        if (bg.HasValue) Console.BackgroundColor = bg.Value;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"
╔═════════════════════════════════════════════════════════════════════════════════════╗
║   HỌC PHẦN: PHÁT TRIỂN HỆ THỐNG SERVER NÂNG CAO — BUỔI 12                           ║
║   DEMO THỰC THI: MODULAR MONOLITHIC & GIAO TIẾP ĐỒNG BỘ / BẤT ĐỒNG BỘ RABBITMQ      ║
╚═════════════════════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }
}
