namespace Modules.Notification.Domain;

public class NotificationLog
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string Channel { get; private set; }
    public string Recipient { get; private set; }
    public string Message { get; private set; }
    public DateTime SentAt { get; private set; }

    public NotificationLog(Guid orderId, string channel, string recipient, string message)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Channel = channel;
        Recipient = recipient;
        Message = message;
        SentAt = DateTime.UtcNow;
    }
}
