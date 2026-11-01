using Inventory.Contracts;
using Inventory.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Order.Application.DTOs;
using Modules.Order.Domain;
using Modules.Order.Infrastructure;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Order.Application;

public class OrderService
{
    private readonly IInventoryService _inventoryService;
    private readonly OrderDbContext _dbContext;
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<OrderService> _logger;

    internal OrderService(
        IInventoryService inventoryService,
        OrderDbContext dbContext,
        IRabbitMqPublisher publisher,
        ILogger<OrderService> logger)
    {
        _inventoryService = inventoryService;
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>
    /// Luồng xử lý đơn hàng phối hợp Sync & Async:
    /// 1. GỌI ĐỒNG BỘ (Sync Call): Gọi IInventoryService.ReserveAsync để giữ hàng nguyên tử trong kho.
    ///    - Nếu hết hàng -> Lưu đơn Rejected và trả về kết quả thất bại (sẽ map HTTP 409 Conflict).
    /// 2. GHI CSDL ĐƠN HÀNG (Local ACID Transaction): Lưu đơn Confirmed vào schema "order".
    /// 3. BẮN SỰ KIỆN BẤT ĐỒNG BỘ (Async Publish): Publish OrderCreatedEvent lên RabbitMQ Exchange "order.events".
    /// </summary>
    public async Task<CreateOrderResponse> PlaceOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var orderId = Guid.NewGuid();
        _logger.LogInformation(
            "[ORDER] [START] Tiếp nhận yêu cầu tạo đơn #{OrderId} cho khách hàng {CustomerId}. Số mặt hàng: {ItemCount}",
            orderId, request.CustomerId, request.Items.Count);

        var reservedItems = new List<(Guid ProductId, int Quantity, string ProductName, decimal UnitPrice)>();

        // =========================================================================
        // BƯỚC 1: GỌI ĐỒNG BỘ (SYNC) -> GIỮ HÀNG NGUYÊN TỬ (RESERVE)
        // =========================================================================
        foreach (var item in request.Items)
        {
            var reserveRequest = new ReserveStockRequest(item.ProductId, item.Quantity, orderId);
            var reserveResult = await _inventoryService.ReserveAsync(reserveRequest, cancellationToken);

            if (!reserveResult.Success)
            {
                _logger.LogWarning(
                    "[ORDER] [SYNC-RESERVE-FAILED] Giữ hàng thất bại cho đơn #{OrderId}: {Reason}",
                    orderId, reserveResult.Message);

                // Hoàn trả các mặt hàng đã lỡ giữ trước đó trong đơn này (Compensating action)
                foreach (var prev in reservedItems)
                {
                    await _inventoryService.ReleaseAsync(new ReleaseStockRequest(prev.ProductId, prev.Quantity, orderId), cancellationToken);
                }

                // Ghi nhận đơn hàng REJECTED vào CSDL để phục vụ lưu vết lịch sử
                var rejectedOrder = new Domain.Order
                {
                    Id = orderId,
                    CustomerId = request.CustomerId,
                    Status = OrderStatus.Rejected,
                    TotalAmount = 0,
                    FailureReason = reserveResult.Message,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Orders.Add(rejectedOrder);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new CreateOrderResponse(
                    Success: false,
                    OrderId: orderId,
                    Status: OrderStatus.Rejected,
                    TotalAmount: 0,
                    Message: reserveResult.Message
                );
            }

            reservedItems.Add((item.ProductId, item.Quantity, reserveResult.ProductName ?? "Sản phẩm", reserveResult.UnitPrice));
        }

        // =========================================================================
        // BƯỚC 2: GHI CSDL ĐƠN HÀNG NỘI BỘ (LOCAL ACID TRANSACTION)
        // =========================================================================
        var confirmedOrder = new Domain.Order
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            Status = OrderStatus.Confirmed,
            TotalAmount = reservedItems.Sum(x => x.Quantity * x.UnitPrice),
            CreatedAt = DateTime.UtcNow
        };

        foreach (var r in reservedItems)
        {
            confirmedOrder.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = r.ProductId,
                ProductNameSnapshot = r.ProductName,
                UnitPriceSnapshot = r.UnitPrice,
                Quantity = r.Quantity
            });
        }

        try
        {
            _dbContext.Orders.Add(confirmedOrder);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[ORDER] [LOCAL-TX-SUCCESS] Đã lưu thành công đơn hàng Confirmed #{OrderId} vào CSDL (Tổng tiền: {Total:N0} VNĐ).",
                orderId, confirmedOrder.TotalAmount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ORDER] [LOCAL-TX-ERROR] Lưu đơn hàng #{OrderId} thất bại, đang nhả hàng giữ...", orderId);
            foreach (var r in reservedItems)
            {
                await _inventoryService.ReleaseAsync(new ReleaseStockRequest(r.ProductId, r.Quantity, orderId), cancellationToken);
            }
            throw;
        }

        // =========================================================================
        // BƯỚC 3: GIAO TIẾP BẤT ĐỒNG BỘ (ASYNC EVENT PUBLISH QUA RABBITMQ)
        // =========================================================================
        var eventItems = confirmedOrder.Items.Select(i => new OrderItemEventDto(
            i.ProductId,
            i.ProductNameSnapshot,
            i.Quantity,
            i.UnitPriceSnapshot
        )).ToList();

        var orderCreatedEvent = new OrderCreatedEvent(
            EventId: Guid.NewGuid(),
            OrderId: confirmedOrder.Id,
            CustomerId: confirmedOrder.CustomerId,
            Items: eventItems,
            TotalAmount: confirmedOrder.TotalAmount,
            OccurredOn: DateTime.UtcNow
        );

        await _publisher.PublishAsync(RabbitMqConstants.OrderEventsExchange, orderCreatedEvent, cancellationToken);
        _logger.LogInformation(
            "[ORDER] [PUBLISH-SUCCESS] Sự kiện OrderCreatedEvent đã được publish lên RabbitMQ Exchange '{Exchange}'.",
            RabbitMqConstants.OrderEventsExchange);

        // =========================================================================
        // BƯỚC 4: PHẢN HỒI CHO KHÁCH HÀNG (HTTP 201)
        // =========================================================================
        var responseItems = confirmedOrder.Items.Select(i => new OrderItemResponse(
            i.ProductId,
            i.ProductNameSnapshot,
            i.Quantity,
            i.UnitPriceSnapshot
        )).ToList();

        return new CreateOrderResponse(
            Success: true,
            OrderId: confirmedOrder.Id,
            Status: OrderStatus.Confirmed,
            TotalAmount: confirmedOrder.TotalAmount,
            Message: "Đặt hàng thành công! Đơn hàng đã được xác nhận và chuyển tiếp xử lý ngầm.",
            Items: responseItems
        );
    }

    public async Task<OrderDetailsResponse?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var o = await _dbContext.Orders
            .Include(x => x.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (o == null) return null;

        return new OrderDetailsResponse(
            o.Id,
            o.CustomerId,
            o.Status.ToString(),
            o.TotalAmount,
            o.FailureReason,
            o.CreatedAt,
            o.Items.Select(i => new OrderItemResponse(i.ProductId, i.ProductNameSnapshot, i.Quantity, i.UnitPriceSnapshot)).ToList()
        );
    }

    public async Task<List<OrderDetailsResponse>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var list = await _dbContext.Orders
            .Include(x => x.Items)
            .OrderByDescending(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return list.Select(o => new OrderDetailsResponse(
            o.Id,
            o.CustomerId,
            o.Status.ToString(),
            o.TotalAmount,
            o.FailureReason,
            o.CreatedAt,
            o.Items.Select(i => new OrderItemResponse(i.ProductId, i.ProductNameSnapshot, i.Quantity, i.UnitPriceSnapshot)).ToList()
        )).ToList();
    }
}
