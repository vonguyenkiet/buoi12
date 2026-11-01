using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;
using Modules.Inventory.Infrastructure;

namespace Modules.Inventory.Consumers;

/// <summary>
/// Consumer lắng nghe sự kiện OrderCreated trên RabbitMQ (Queue: order.inventory).
/// Thực thi bất đồng bộ: Trừ tồn kho vật lý sau khi đơn hàng đã chốt.
/// </summary>
public class InventoryOrderCreatedConsumer
{
    private readonly InventoryDbContext _dbContext;
    private readonly Action<string>? _logger;

    internal InventoryOrderCreatedConsumer(InventoryDbContext dbContext, Action<string>? logger = null)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> HandleAsync(OrderCreatedEvent @event)
    {
        _logger?.Invoke($"[Inventory-Consumer] Nhận sự kiện OrderCreated (OrderId: {@event.OrderId}) từ RabbitMQ Queue 'order.inventory'");

        foreach (var item in @event.Items)
        {
            var stock = _dbContext.FindByProductId(item.ProductId);
            if (stock != null)
            {
                stock.DeductStock(item.Quantity);
                _dbContext.Save(stock);
                _logger?.Invoke($" -> [Inventory-DB] Đã trừ {item.Quantity} chiếc '{stock.ProductName}'. Tồn kho mới còn: {stock.AvailableQuantity}");
            }
        }

        // Trả về true để gửi Manual ACK (channel.basicAck) báo cáo đã xử lý xong và commit DB an toàn
        return await Task.FromResult(true);
    }
}
