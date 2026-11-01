namespace Modules.Inventory.Domain;

/// <summary>
/// Domain Entity nội bộ của Inventory Bounded Context.
/// CẤM TUYỆT ĐỐI module khác tham chiếu hoặc truy vấn trực tiếp Entity này.
/// </summary>
public class StockItem
{
    public Guid ProductId { get; private set; }
    public string Sku { get; private set; }
    public string ProductName { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }

    public StockItem(Guid productId, string sku, string productName, int initialQuantity)
    {
        ProductId = productId;
        Sku = sku;
        ProductName = productName;
        AvailableQuantity = initialQuantity;
        ReservedQuantity = 0;
    }

    public bool HasSufficientStock(int requestedQuantity)
    {
        return AvailableQuantity >= requestedQuantity;
    }

    public bool DeductStock(int quantity)
    {
        if (!HasSufficientStock(quantity))
            return false;

        AvailableQuantity -= quantity;
        return true;
    }

    public void Restock(int quantity)
    {
        if (quantity > 0)
            AvailableQuantity += quantity;
    }
}
