using Modules.Order.Domain;

namespace Modules.Order.Application.DTOs;

public record OrderItemRequestDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemRequestDto> Items
);

public record CreateOrderResult(
    bool Success,
    Guid? OrderId,
    OrderStatus? Status,
    decimal TotalAmount,
    string Message
);
