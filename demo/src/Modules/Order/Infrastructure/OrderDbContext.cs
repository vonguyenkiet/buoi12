using System.Collections.Concurrent;
using System.Text.Json;
using OrderEntity = Modules.Order.Domain.Order;
using Modules.Order.Domain;

namespace Modules.Order.Infrastructure;

/// <summary>
/// Infrastructure Layer nội bộ của Order Module.
/// Được đánh dấu INTERNAL để ngăn chặn chia sẻ DbContext giữa các module.
/// Lưu trữ độc lập trong data/order/order_schema.json.
/// </summary>
internal class OrderDbContext
{
    private readonly string _storagePath;
    private readonly ConcurrentDictionary<Guid, OrderEntity> _orders = new();
    private readonly object _lock = new();

    public OrderDbContext(string baseDataDir = "data/order")
    {
        Directory.CreateDirectory(baseDataDir);
        _storagePath = Path.Combine(baseDataDir, "order_schema.json");
        LoadFromDisk();
    }

    private void LoadFromDisk()
    {
        lock (_lock)
        {
            if (File.Exists(_storagePath))
            {
                var json = File.ReadAllText(_storagePath);
                var items = JsonSerializer.Deserialize<List<OrderRecord>>(json);
                if (items != null)
                {
                    _orders.Clear();
                    foreach (var it in items)
                    {
                        var ord = new OrderEntity(it.CustomerId);
                        // Reflection or direct mapping
                        typeof(OrderEntity).GetProperty(nameof(OrderEntity.Id))?.SetValue(ord, it.Id);
                        typeof(OrderEntity).GetProperty(nameof(OrderEntity.Status))?.SetValue(ord, it.Status);
                        typeof(OrderEntity).GetProperty(nameof(OrderEntity.TotalAmount))?.SetValue(ord, it.TotalAmount);
                        typeof(OrderEntity).GetProperty(nameof(OrderEntity.CreatedAt))?.SetValue(ord, it.CreatedAt);
                        foreach (var l in it.Items)
                        {
                            ord.AddItem(l.ProductId, l.ProductNameSnapshot, l.Quantity, l.UnitPriceSnapshot);
                        }
                        _orders[ord.Id] = ord;
                    }
                }
            }
        }
    }

    public void SaveToDisk()
    {
        lock (_lock)
        {
            var records = _orders.Values.Select(o => new OrderRecord(
                o.Id,
                o.CustomerId,
                o.Status,
                o.TotalAmount,
                o.CreatedAt,
                o.Items.Select(i => new OrderLineRecord(i.Id, i.ProductId, i.ProductNameSnapshot, i.UnitPriceSnapshot, i.Quantity)).ToList()
            )).ToList();

            var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storagePath, json);
        }
    }

    public void Save(OrderEntity order)
    {
        _orders[order.Id] = order;
        SaveToDisk();
    }

    public OrderEntity? FindById(Guid id)
    {
        _orders.TryGetValue(id, out var ord);
        return ord;
    }

    public IEnumerable<OrderEntity> GetAll() => _orders.Values;

    private record OrderLineRecord(Guid Id, Guid ProductId, string ProductNameSnapshot, decimal UnitPriceSnapshot, int Quantity);
    private record OrderRecord(Guid Id, Guid CustomerId, OrderStatus Status, decimal TotalAmount, DateTime CreatedAt, List<OrderLineRecord> Items);
}
