using Inventory.Contracts;
using Inventory.Contracts.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Inventory.Application;
using Modules.Inventory.Consumers;
using Modules.Inventory.Domain;
using Modules.Inventory.Infrastructure;

namespace Modules.Inventory;

public static class InventoryModuleExtensions
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionString)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__EFMigrationsHistory", "inventory")));

        // Đăng ký implementation InProcess nội bộ
        services.AddScoped<InProcessInventoryService>();

        // Cấu hình HttpClient cho HttpInventoryService (loopback trong Modular Monolith, hoặc service ngoài khi tách Microservice)
        var inventoryHttpUrl = configuration["Inventory:HttpUrl"] ?? "http://localhost:5000";
        services.AddHttpClient<HttpInventoryService>(client =>
        {
            client.BaseAddress = new Uri(inventoryHttpUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        // Chọn adapter cho IInventoryService qua config "Inventory:Mode"
        var mode = configuration["Inventory:Mode"] ?? "InProcess";
        if (mode.Equals("Http", StringComparison.OrdinalIgnoreCase))
        {
            services.AddScoped<IInventoryService>(sp => sp.GetRequiredService<HttpInventoryService>());
        }
        else
        {
            services.AddScoped<IInventoryService>(sp => sp.GetRequiredService<InProcessInventoryService>());
        }

        // Đăng ký RabbitMQ Consumer
        services.AddHostedService<InventoryOrderCreatedConsumer>();

        return services;
    }

    public static async Task SeedInventoryDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("InventorySeed");

        await db.Database.EnsureCreatedAsync();

        var laptopId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var mouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var lastLaptopId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var laptop = await db.Stocks.FindAsync(laptopId);
        if (laptop == null)
        {
            db.Stocks.Add(new Stock
            {
                ProductId = laptopId,
                Sku = "SKU-LAPTOP-PRO",
                Name = "Laptop Gaming Pro 16",
                Price = 25000000m,
                AvailableQuantity = 5,
                ReservedQuantity = 0,
                SoldQuantity = 0
            });
        }

        var mouse = await db.Stocks.FindAsync(mouseId);
        if (mouse == null)
        {
            db.Stocks.Add(new Stock
            {
                ProductId = mouseId,
                Sku = "SKU-MOUSE-RGB",
                Name = "Chuột Không Dây RGB",
                Price = 450000m,
                AvailableQuantity = 0, // HẾT HÀNG (để demo 409 Rejected)
                ReservedQuantity = 0,
                SoldQuantity = 0
            });
        }

        var lastLaptop = await db.Stocks.FindAsync(lastLaptopId);
        if (lastLaptop == null)
        {
            db.Stocks.Add(new Stock
            {
                ProductId = lastLaptopId,
                Sku = "SKU-LAPTOP-LAST",
                Name = "Laptop Gaming Cuối Cùng",
                Price = 30000000m,
                AvailableQuantity = 1, // CHỈ CÒN 1 (để demo race.ps1 chống bán vượt tồn)
                ReservedQuantity = 0,
                SoldQuantity = 0
            });
        }

        await db.SaveChangesAsync();
        logger.LogInformation("[INVENTORY] ✔ Đã kiểm tra và khởi tạo dữ liệu mẫu kho hàng.");
    }

    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Public API xem tồn kho
        endpoints.MapGet("/api/inventory/{productId:guid}", async (Guid productId, InProcessInventoryService service) =>
        {
            var stock = await service.GetStockAsync(productId);
            return stock != null ? Results.Ok(stock) : Results.NotFound(new { Message = "Sản phẩm không tồn tại trong kho." });
        });

        // Internal REST endpoints được gọi bởi HttpInventoryService (Loopback hoặc Microservice)
        endpoints.MapPost("/internal/inventory/reserve", async (ReserveStockRequest request, InProcessInventoryService service) =>
        {
            var result = await service.ReserveAsync(request);
            return result.Success ? Results.Ok(result) : Results.Conflict(result);
        });

        endpoints.MapPost("/internal/inventory/release", async (ReleaseStockRequest request, InProcessInventoryService service) =>
        {
            var success = await service.ReleaseAsync(request);
            return success ? Results.Ok(new { Success = true }) : Results.BadRequest(new { Success = false });
        });

        endpoints.MapGet("/internal/inventory/{productId:guid}", async (Guid productId, InProcessInventoryService service) =>
        {
            var stock = await service.GetStockAsync(productId);
            return stock != null ? Results.Ok(stock) : Results.NotFound();
        });

        return endpoints;
    }
}
