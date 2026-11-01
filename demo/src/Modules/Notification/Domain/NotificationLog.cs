namespace Modules.Notification.Domain;

public class NotificationLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public Guid EventId { get; set; }
    public string Channel { get; set; } = "EMAIL"; // EMAIL, SMS
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsSimulation { get; set; } = true; // Ghi rõ là mô phỏng
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class NotificationProcessedEvent
{
    public Guid EventId { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}
