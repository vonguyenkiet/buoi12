namespace Shared.Kernel.Events;

public record OrderItemEventDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record OrderCreatedEvent(
    Guid EventId,
    Guid OrderId,
    Guid CustomerId,
    List<OrderItemEventDto> Items,
    decimal TotalAmount,
    DateTime OccurredOn
);
