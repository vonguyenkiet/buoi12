using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Notification.Consumers;
using Modules.Notification.Infrastructure;

namespace Modules.Notification;

public static class NotificationModuleExtensions
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__EFMigrationsHistory", "notification")));

        services.AddSingleton<NotificationOrderCreatedConsumer>();
        services.AddHostedService(sp => sp.GetRequiredService<NotificationOrderCreatedConsumer>());

        return services;
    }

    public static async Task SeedNotificationDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("NotificationSeed");

        await db.Database.EnsureCreatedAsync();
        logger.LogInformation("[NOTIFICATION] ✔ Đã kiểm tra và đảm bảo schema/tables cho Notification.");
    }

    public static async Task ResetNotificationDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        db.NotificationLogs.RemoveRange(db.NotificationLogs);
        db.ProcessedEvents.RemoveRange(db.ProcessedEvents);
        await db.SaveChangesAsync();
    }

    public static IEndpointRouteBuilder MapNotificationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Public API xem danh sách thông báo đã gửi
        endpoints.MapGet("/api/notifications", async (NotificationDbContext db) =>
        {
            var logs = await db.NotificationLogs
                .OrderByDescending(n => n.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
            return Results.Ok(logs);
        });

        // Demo endpoints: Dừng / Bật Consumer Notification để kiểm chứng tính tin cậy RabbitMQ
        endpoints.MapPost("/demo/consumers/notification/stop", async (NotificationOrderCreatedConsumer consumer) =>
        {
            await consumer.StopConsumerAsync();
            return Results.Ok(new
            {
                Status = "STOPPED",
                IsActive = consumer.IsActive,
                Message = "Notification Consumer đã dừng! Các thông điệp OrderCreated mới sẽ nằm chờ trong hàng đợi RabbitMQ (Ready > 0)."
            });
        });

        endpoints.MapPost("/demo/consumers/notification/start", async (NotificationOrderCreatedConsumer consumer) =>
        {
            await consumer.StartConsumerAsync();
            return Results.Ok(new
            {
                Status = "RUNNING",
                IsActive = consumer.IsActive,
                Message = "Notification Consumer đã bật lại! Đang tiêu thụ và xử lý toàn bộ thông điệp tồn đọng trong queue."
            });
        });

        endpoints.MapGet("/demo/consumers/notification/status", (NotificationOrderCreatedConsumer consumer) =>
        {
            return Results.Ok(new
            {
                Consumer = "notification.order-created",
                IsActive = consumer.IsActive,
                State = consumer.IsActive ? "ONLINE" : "OFFLINE"
            });
        });

        return endpoints;
    }
}
