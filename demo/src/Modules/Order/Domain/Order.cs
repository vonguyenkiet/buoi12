namespace Modules.Order.Domain;

public enum OrderStatus
{
    Pending,
    Confirmed,
    Rejected
}

/// <summary>
/// Domain Entity Order (Aggregate Root).
/// </summary>
public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public List<OrderLineItem> Items { get; private set; } = new();

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        var lineItem = new OrderLineItem(productId, productName, quantity, unitPrice);
        Items.Add(lineItem);
        TotalAmount += (quantity * unitPrice);
    }

    public void Confirm()
    {
        Status = OrderStatus.Confirmed;
    }

    public void Reject(string reason)
    {
        Status = OrderStatus.Rejected;
    }
}

/// <summary>
/// OrderLineItem thể hiện tính bất biến của Snapshot Pattern:
/// Lưu ProductNameSnapshot và UnitPriceSnapshot tại thời điểm đặt hàng.
/// CẤM TUYỆT ĐỐI SQL JOIN sang Catalog Module hay Inventory Module.
/// </summary>
public class OrderLineItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductNameSnapshot { get; private set; }
    public decimal UnitPriceSnapshot { get; private set; }
    public int Quantity { get; private set; }

    public OrderLineItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductNameSnapshot = productName;
        Quantity = quantity;
        UnitPriceSnapshot = unitPrice;
    }
}
