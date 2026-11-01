using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Order.Application;
using Modules.Order.Application.DTOs;
using Modules.Order.Infrastructure;

namespace Modules.Order;

public static class OrderModuleExtensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__EFMigrationsHistory", "order")));

        // Đăng ký OrderService qua Factory lambda (giữ constructor internal)
        services.AddScoped(sp => new OrderService(
            sp.GetRequiredService<Inventory.Contracts.IInventoryService>(),
            sp.GetRequiredService<OrderDbContext>(),
            sp.GetRequiredService<Shared.Kernel.RabbitMQ.IRabbitMqPublisher>(),
            sp.GetRequiredService<ILogger<OrderService>>()));

        return services;
    }

    public static async Task SeedOrderDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("OrderSeed");

        await db.Database.EnsureCreatedAsync();
        logger.LogInformation("[ORDER] ✔ Đã kiểm tra và đảm bảo schema/tables cho Order.");
    }

    public static async Task ResetOrderDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        db.OrderItems.RemoveRange(db.OrderItems);
        db.Orders.RemoveRange(db.Orders);
        await db.SaveChangesAsync();
    }

    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/orders", async (CreateOrderRequest request, OrderService service) =>
        {
            var result = await service.PlaceOrderAsync(request);
            if (!result.Success)
            {
                return Results.Json(result, statusCode: StatusCodes.Status409Conflict);
            }

            return Results.Created($"/api/orders/{result.OrderId}", result);
        });

        endpoints.MapGet("/api/orders/{id:guid}", async (Guid id, OrderService service) =>
        {
            var order = await service.GetOrderByIdAsync(id);
            return order != null ? Results.Ok(order) : Results.NotFound(new { Message = "Đơn hàng không tồn tại." });
        });

        endpoints.MapGet("/api/orders", async (OrderService service) =>
        {
            var orders = await service.GetAllOrdersAsync();
            return Results.Ok(orders);
        });

        return endpoints;
    }
}
