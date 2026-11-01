using Microsoft.EntityFrameworkCore;
using Modules.Inventory;
using Modules.Inventory.Application;
using Modules.Inventory.Domain;
using Modules.Notification;
using Modules.Notification.Consumers;
using Modules.Order;
using Shared.Kernel.RabbitMQ;

// Nạp cấu hình từ .env cục bộ nếu có (tuyệt đối không commit file .env chứa mật khẩu lên git)
LoadDotEnv();

var builder = WebApplication.CreateBuilder(args);

static void LoadDotEnv()
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (dir != null)
    {
        var envFile = Path.Combine(dir.FullName, ".env");
        if (File.Exists(envFile))
        {
            foreach (var line in File.ReadAllLines(envFile))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#")) continue;
                var parts = trimmed.Split('=', 2);
                if (parts.Length == 2 && string.IsNullOrEmpty(Environment.GetEnvironmentVariable(parts[0].Trim())))
                {
                    Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
                }
            }
            break;
        }
        dir = dir.Parent;
    }
}

// 1. Cấu hình Connection String PostgreSQL
var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? builder.Configuration["ConnectionStrings:Host"] ?? "localhost";
var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? builder.Configuration["ConnectionStrings:Port"] ?? "5432";
var pgDb = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? builder.Configuration["ConnectionStrings:Database"] ?? "buoi12_db";
var pgUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? builder.Configuration["ConnectionStrings:Username"] ?? "postgres";
var pgPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? builder.Configuration["ConnectionStrings:Password"] ?? "postgres";

var connectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPass};";

// 2. Cấu hình RabbitMQ Connection
var rmqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? builder.Configuration["RabbitMQ:Host"] ?? "localhost";
var rmqPort = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? builder.Configuration["RabbitMQ:Port"], out var p) ? p : 5672;
var rmqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? builder.Configuration["RabbitMQ:Username"] ?? "guest";
var rmqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? builder.Configuration["RabbitMQ:Password"] ?? "guest";

builder.Services.AddSingleton<IRabbitMqConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMqConnection>>();
    return new RabbitMqConnection(rmqHost, rmqPort, rmqUser, rmqPass, logger);
});

builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

// 3. Đăng ký 3 Module độc lập (Mỗi module có schema riêng biệt)
builder.Services.AddInventoryModule(builder.Configuration, connectionString);
builder.Services.AddOrderModule(connectionString);
builder.Services.AddNotificationModule(connectionString);

var app = builder.Build();

// 4. Khởi tạo CSDL, Schemas, và Topology RabbitMQ lúc Startup
using (var scope = app.Services.CreateScope())
{
    var rmq = scope.ServiceProvider.GetRequiredService<IRabbitMqConnection>();
    await rmq.InitializeTopologyAsync();

    await InventoryModuleExtensions.SeedInventoryDataAsync(app.Services);
    await OrderModuleExtensions.SeedOrderDataAsync(app.Services);
    await NotificationModuleExtensions.SeedNotificationDataAsync(app.Services);
}

// 5. Đăng ký Endpoints cho các Module
app.MapInventoryEndpoints();
app.MapOrderEndpoints();
app.MapNotificationEndpoints();

// Root API: Giới thiệu hệ thống
app.MapGet("/", (IConfiguration config) => Results.Ok(new
{
    Title = "Modular Monolithic E-Commerce Server — Buổi 12",
    Architecture = "Modular Monolithic (.NET 10, PostgreSQL 3 schemas, RabbitMQ AMQP)",
    InventorySyncMode = config["Inventory:Mode"] ?? "InProcess",
    Modules = new[] { "Order", "Inventory", "Notification" },
    Databases = new
    {
        Type = "PostgreSQL 16 (Single Physical Server)",
        Schemas = new[] { "inventory", "order", "notification" }
    },
    MessageBroker = new
    {
        Type = "RabbitMQ AMQP 0-9-1",
        Exchange = "order.events (Fanout, Durable)",
        Queues = new[] { "inventory.order-created", "notification.order-created" }
    }
}));

// Demo API: Trạng thái hàng đợi & CSDL
app.MapGet("/demo/status", async (IRabbitMqConnection rmq, NotificationOrderCreatedConsumer notifConsumer, InProcessInventoryService inv) =>
{
    var laptop = await inv.GetStockAsync(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    var mouse = await inv.GetStockAsync(Guid.Parse("22222222-2222-2222-2222-222222222222"));
    var lastLaptop = await inv.GetStockAsync(Guid.Parse("33333333-3333-3333-3333-333333333333"));

    uint invCount = await rmq.GetMessageCountAsync(RabbitMqConstants.InventoryOrderCreatedQueue);
    uint notifCount = await rmq.GetMessageCountAsync(RabbitMqConstants.NotificationOrderCreatedQueue);
    uint dlqCount = await rmq.GetMessageCountAsync(RabbitMqConstants.DeadLetterQueue);

    return Results.Ok(new
    {
        NotificationConsumer = new
        {
            Queue = RabbitMqConstants.NotificationOrderCreatedQueue,
            IsOnline = notifConsumer.IsActive,
            Status = notifConsumer.IsActive ? "ONLINE" : "OFFLINE"
        },
        RabbitMqQueues = new
        {
            InventoryQueue_Depth = invCount,
            NotificationQueue_Depth = notifCount,
            DeadLetterQueue_Depth = dlqCount
        },
        InventoryStocks = new[] { laptop, mouse, lastLaptop }
    });
});

// Demo API: Reset CSDL và Hàng đợi về trạng thái ban đầu
app.MapPost("/demo/reset", async (IServiceProvider sp) =>
{
    using var scope = sp.CreateScope();
    var rmq = scope.ServiceProvider.GetRequiredService<IRabbitMqConnection>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("[DEMO-RESET] Đang reset dữ liệu demo về trạng thái ban đầu...");

    // Purge queues
    await rmq.PurgeQueueAsync(RabbitMqConstants.InventoryOrderCreatedQueue);
    await rmq.PurgeQueueAsync(RabbitMqConstants.NotificationOrderCreatedQueue);
    await rmq.PurgeQueueAsync(RabbitMqConstants.DeadLetterQueue);

    // Reset inventory stocks in DB
    await InventoryModuleExtensions.ResetInventoryDataAsync(sp);

    // Reset orders
    await OrderModuleExtensions.ResetOrderDataAsync(sp);

    // Reset notification logs
    await NotificationModuleExtensions.ResetNotificationDataAsync(sp);

    logger.LogInformation("[DEMO-RESET] ✔ Dữ liệu đã được reset: Laptop=5, Chuột=0, Laptop-cuối=1, Hàng đợi trống.");
    return Results.Ok(new
    {
        Success = true,
        Message = "Dữ liệu demo đã được reset thành công: Laptop=5, Chuột=0, Laptop-cuối=1."
    });
});

app.Run();

// Public class for WebApplicationFactory in tests
public partial class Program { }
