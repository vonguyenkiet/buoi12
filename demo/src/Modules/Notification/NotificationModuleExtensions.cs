using Modules.Notification.Consumers;
using Modules.Notification.Infrastructure;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Notification;

public static class NotificationModuleExtensions
{
    public static NotificationOrderCreatedConsumer InitializeNotificationModule(
        IRabbitMqBroker broker,
        string dataDir = "data/notification",
        Action<string>? logger = null)
    {
        var dbContext = new NotificationDbContext(dataDir);
        var consumer = new NotificationOrderCreatedConsumer(dbContext, logger);

        // RabbitMQ Configuration:
        // Khai báo Queue cho Notification Module và bind vào Fanout Exchange
        broker.DeclareQueue("order.notification", durable: true);
        broker.BindQueue("order.notification", "ecommerce.order-events");

        // Đăng ký Consumer với Manual ACK
        broker.Subscribe<OrderCreatedEvent>("order.notification", consumer.HandleAsync, autoAck: false);

        return consumer;
    }
}
