using Modules.Notification.Domain;
using Modules.Notification.Infrastructure;
using Shared.Kernel.Events;

namespace Modules.Notification.Consumers;

/// <summary>
/// Consumer của Notification Module lắng nghe sự kiện trên Queue 'order.notification'.
/// Xử lý độc lập với Order và Inventory.
/// </summary>
public class NotificationOrderCreatedConsumer
{
    private readonly NotificationDbContext _dbContext;
    private readonly Action<string>? _logger;

    internal NotificationOrderCreatedConsumer(NotificationDbContext dbContext, Action<string>? logger = null)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(OrderCreatedEvent @event)
    {
        _logger?.Invoke($"[Notification-Consumer] Nhận sự kiện OrderCreated (OrderId: {@event.OrderId}) từ RabbitMQ Queue 'order.notification'");

        // Giả lập gửi Email cho khách hàng
        var emailMessage = $"Xác nhận đặt hàng: Đơn #{Path.GetFileNameWithoutExtension(@event.OrderId.ToString()[..8])} tổng tiền {@event.TotalAmount:N0} VNĐ với {@event.Items.Count} sản phẩm đã được tiếp nhận!";
        var emailLog = new NotificationLog(@event.OrderId, "EMAIL", $"customer_{@event.CustomerId.ToString()[..6]}@gmail.com", emailMessage);
        _dbContext.AddLog(emailLog);

        _logger?.Invoke($" -> [Notification-Service] Đã gửi EMAIL xác nhận đơn hàng thành công tới: customer_{@event.CustomerId.ToString()[..6]}@gmail.com");

        // Giả lập gửi SMS
        var smsLog = new NotificationLog(@event.OrderId, "SMS", "0908123456", $"Ma don {@event.OrderId.ToString()[..8]} da duoc tao thanh cong.");
        _dbContext.AddLog(smsLog);
        _logger?.Invoke($" -> [Notification-Service] Đã gửi SMS thông báo đơn hàng thành công!");

        // Manual ACK
        return await Task.FromResult(true);
    }

    public IEnumerable<NotificationLog> GetLogs() => _dbContext.GetAll();
}
