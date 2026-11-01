namespace Shared.Kernel.RabbitMQ;

public static class RabbitMqConstants
{
    public const string OrderEventsExchange = "order.events";
    public const string DeadLetterExchange = "order.dlx";
    public const string DeadLetterQueue = "order.dead-letter";
    public const string DeadLetterRoutingKey = "order.dead-letter";

    public const string InventoryOrderCreatedQueue = "inventory.order-created";
    public const string NotificationOrderCreatedQueue = "notification.order-created";

    public const string HeaderRetryCount = "x-retry-count";
    public const int MaxRetryCount = 3;
}
