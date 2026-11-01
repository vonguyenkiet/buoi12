using System.Collections.Concurrent;
using System.Text.Json;
using Modules.Inventory.Domain;

namespace Modules.Inventory.Infrastructure;

/// <summary>
/// Infrastructure Layer nội bộ của Inventory Module.
/// Được khai báo INTERNAL để ngăn chặn mọi hành vi vi phạm kiến trúc (truy cập chéo DbContext).
/// Dữ liệu lưu trữ độc lập trong khoang lưu trữ riêng biệt của Inventory Module.
/// </summary>
internal class InventoryDbContext
{
    private readonly string _storagePath;
    private readonly ConcurrentDictionary<Guid, StockItem> _stocks = new();
    private readonly object _lock = new();

    public InventoryDbContext(string baseDataDir = "data/inventory")
    {
        Directory.CreateDirectory(baseDataDir);
        _storagePath = Path.Combine(baseDataDir, "inventory_schema.json");
        LoadFromDisk();
    }

    private void LoadFromDisk()
    {
        lock (_lock)
        {
            if (File.Exists(_storagePath))
            {
                var json = File.ReadAllText(_storagePath);
                var items = JsonSerializer.Deserialize<List<StockItemRecord>>(json);
                if (items != null)
                {
                    _stocks.Clear();
                    foreach (var it in items)
                    {
                        _stocks[it.ProductId] = new StockItem(it.ProductId, it.Sku, it.ProductName, it.AvailableQuantity);
                    }
                }
            }
            else
            {
                // Khởi tạo dữ liệu mẫu cho bài toán E-commerce
                SeedData();
            }
        }
    }

    private void SeedData()
    {
        var laptopId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var mouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        _stocks[laptopId] = new StockItem(laptopId, "SKU-LAPTOP-PRO", "Laptop Gaming Pro 16", 5);
        _stocks[mouseId] = new StockItem(mouseId, "SKU-MOUSE-RGB", "Chuột Không Dây RGB", 0); // HẾT HÀNG (để test)

        SaveToDisk();
    }

    public void SaveToDisk()
    {
        lock (_lock)
        {
            var records = _stocks.Values.Select(s => new StockItemRecord(
                s.ProductId, s.Sku, s.ProductName, s.AvailableQuantity, s.ReservedQuantity
            )).ToList();

            var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storagePath, json);
        }
    }

    public StockItem? FindByProductId(Guid productId)
    {
        _stocks.TryGetValue(productId, out var item);
        return item;
    }

    public IEnumerable<StockItem> GetAll() => _stocks.Values;

    public void Save(StockItem item)
    {
        _stocks[item.ProductId] = item;
        SaveToDisk();
    }

    private record StockItemRecord(Guid ProductId, string Sku, string ProductName, int AvailableQuantity, int ReservedQuantity);
}
