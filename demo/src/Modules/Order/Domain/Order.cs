namespace Modules.Order.Domain;

public enum OrderStatus
{
    Confirmed,
    Rejected
}

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string? FailureReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new();
}

/// <summary>
/// Snapshot Pattern: Lưu trữ bản chụp bất biến của sản phẩm tại thời điểm mua.
/// Cấm tuyệt đối SQL JOIN sang schema catalog hay inventory.
/// </summary>
public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductNameSnapshot { get; set; } = string.Empty;
    public decimal UnitPriceSnapshot { get; set; }
    public int Quantity { get; set; }
}
