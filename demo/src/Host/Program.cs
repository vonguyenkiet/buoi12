using Modules.Inventory;
using Modules.Inventory.Contracts.DTOs;
using Modules.Notification;
using Modules.Notification.Consumers;
using Modules.Order;
using Modules.Order.Application;
using Modules.Order.Application.DTOs;
using Shared.Kernel.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Khởi tạo RabbitMQ Broker chung
var broker = new RabbitMqBroker(msg => Console.WriteLine($"[RabbitMQ-Web] {msg}"));

// Khởi tạo các Modules trong Modular Monolith
var dataDir = Path.Combine(AppContext.BaseDirectory, "web_data");
Directory.CreateDirectory(dataDir);

var (inventoryService, invConsumer) = InventoryModuleExtensions.InitializeInventoryModule(
    broker, Path.Combine(dataDir, "inventory"), msg => Console.WriteLine($"[Inv-Web] {msg}"));

var notifConsumer = NotificationModuleExtensions.InitializeNotificationModule(
    broker, Path.Combine(dataDir, "notification"), msg => Console.WriteLine($"[Notif-Web] {msg}"));

var orderService = OrderModuleExtensions.InitializeOrderModule(
    inventoryService, broker, Path.Combine(dataDir, "order"), msg => Console.WriteLine($"[Order-Web] {msg}"));

builder.Services.AddSingleton(inventoryService);
builder.Services.AddSingleton(orderService);
builder.Services.AddSingleton(notifConsumer);
builder.Services.AddSingleton<IRabbitMqBroker>(broker);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Title = "Modular Monolithic E-Commerce Server - Buổi 12",
    Architecture = "Modular Monolithic (Single Process, Isolated Schemas, In-Process Seams + RabbitMQ Pub/Sub)",
    Modules = new[] { "Order", "Inventory", "Notification" },
    Endpoints = new[]
    {
        "POST /api/orders -> Tạo đơn hàng (Gọi đồng bộ kiểm tra kho, nếu đủ hàng bắn sự kiện OrderCreated lên RabbitMQ)",
        "GET /api/inventory/{productId} -> Kiểm tra tồn kho sản phẩm",
        "POST /api/inventory/restock -> Nhập kho",
        "GET /api/notifications -> Danh sách thông báo đã nhận từ RabbitMQ Event",
        "POST /api/demo/offline-consumer-toggle -> Bật/Tắt Consumer Notification để test tính tin cậy RabbitMQ"
    }
}));

// API: Tạo đơn hàng
app.MapPost("/api/orders", async (CreateOrderRequest request, OrderService orders) =>
{
    var result = await orders.PlaceOrderAsync(request);
    if (!result.Success)
    {
        return Results.BadRequest(new
        {
            Error = "OUT_OF_STOCK",
            Message = result.Message,
            OrderId = result.OrderId,
            Status = result.Status.ToString()
        });
    }

    return Results.Created($"/api/orders/{result.OrderId}", new
    {
        Message = result.Message,
        OrderId = result.OrderId,
        Status = result.Status.ToString(),
        TotalAmount = result.TotalAmount
    });
});

// API: Kiểm tra tồn kho
app.MapGet("/api/inventory/{productId:guid}", async (Guid productId, Modules.Inventory.Contracts.IInventoryService inventory) =>
{
    var info = await inventory.GetStockInfoAsync(productId);
    return Results.Ok(info);
});

// API: Nhập thêm kho
app.MapPost("/api/inventory/restock", async (Guid productId, int quantity, Modules.Inventory.Contracts.IInventoryService inventory) =>
{
    var ok = await inventory.RestockAsync(productId, quantity);
    return ok ? Results.Ok(new { Message = $"Đã nhập thêm {quantity} sản phẩm." }) : Results.NotFound();
});

// API: Xem danh sách log thông báo
app.MapGet("/api/notifications", (NotificationOrderCreatedConsumer consumer) =>
{
    return Results.Ok(consumer.GetLogs());
});

// API: Bật/Tắt Consumer Notification để thực nghiệm Offline
app.MapPost("/api/demo/offline-consumer-toggle", (IRabbitMqBroker rabbit) =>
{
    const string queueName = "order.notification";
    if (rabbit.IsConsumerOnline(queueName))
    {
        rabbit.DisconnectConsumer(queueName);
        return Results.Ok(new { Status = "OFFLINE", Message = "Notification Consumer đã TẮT. Các sự kiện mới sẽ được lưu trên đĩa RabbitMQ!" });
    }
    else
    {
        rabbit.ReconnectConsumer(queueName);
        return Results.Ok(new { Status = "ONLINE", Message = "Notification Consumer đã BẬT lại. Các sự kiện tồn đọng đã được xử lý!" });
    }
});

app.Run();
