namespace Inventory.Contracts.DTOs;

public record ReserveStockRequest(
    Guid ProductId,
    int Quantity,
    Guid OrderId
);

public record ReserveStockResult(
    bool Success,
    string Message,
    int AvailableQuantity,
    int ReservedQuantity,
    string? ProductName = null,
    decimal UnitPrice = 0m
);

public record ReleaseStockRequest(
    Guid ProductId,
    int Quantity,
    Guid OrderId
);

public record StockDto(
    Guid ProductId,
    string Sku,
    string Name,
    decimal Price,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity
);
