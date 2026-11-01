using Modules.Order.Domain;

namespace Modules.Order.Application.DTOs;

public record OrderItemRequest(
    Guid ProductId,
    int Quantity
);

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequest> Items
);

public record CreateOrderResponse(
    bool Success,
    Guid OrderId,
    OrderStatus Status,
    decimal TotalAmount,
    string Message,
    List<OrderItemResponse>? Items = null
);

public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record OrderDetailsResponse(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string? FailureReason,
    DateTime CreatedAt,
    List<OrderItemResponse> Items
);
