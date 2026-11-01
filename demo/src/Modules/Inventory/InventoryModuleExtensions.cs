using Modules.Inventory.Application;
using Modules.Inventory.Consumers;
using Modules.Inventory.Contracts;
using Modules.Inventory.Infrastructure;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Inventory;

public static class InventoryModuleExtensions
{
    public static (IInventoryService Service, InventoryOrderCreatedConsumer Consumer) InitializeInventoryModule(
        IRabbitMqBroker broker,
        string dataDir = "data/inventory",
        Action<string>? logger = null)
    {
        var dbContext = new InventoryDbContext(dataDir);
        var service = new InventoryService(dbContext);
        var consumer = new InventoryOrderCreatedConsumer(dbContext, logger);

        // RabbitMQ Configuration:
        // Khai báo Queue riêng cho Inventory Module và bind vào Fanout Exchange
        broker.DeclareQueue("order.inventory", durable: true);
        broker.BindQueue("order.inventory", "ecommerce.order-events");

        // Đăng ký Consumer với Manual ACK (autoAck = false)
        broker.Subscribe<OrderCreatedEvent>("order.inventory", consumer.HandleAsync, autoAck: false);

        return (service, consumer);
    }
}
