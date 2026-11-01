using Modules.Inventory.Contracts;
using Modules.Order.Application;
using Modules.Order.Infrastructure;
using Shared.Kernel.RabbitMQ;

namespace Modules.Order;

public static class OrderModuleExtensions
{
    public static OrderService InitializeOrderModule(
        IInventoryService inventoryService,
        IRabbitMqBroker broker,
        string dataDir = "data/order",
        Action<string>? logger = null)
    {
        var dbContext = new OrderDbContext(dataDir);
        var orderService = new OrderService(inventoryService, dbContext, broker, logger);

        // Đảm bảo Fanout Exchange đã được khởi tạo
        broker.DeclareExchange("ecommerce.order-events", type: "fanout", durable: true);

        return orderService;
    }
}
