using Modules.Inventory.Contracts;
using Modules.Inventory.Contracts.DTOs;
using Modules.Order.Application.DTOs;
using Modules.Order.Domain;
using Modules.Order.Infrastructure;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Order.Application;

public class OrderService
{
    private readonly IInventoryService _inventoryService;
    private readonly OrderDbContext _orderDbContext;
    private readonly IRabbitMqBroker _broker;
    private readonly Action<string>? _logger;

    internal OrderService(
        IInventoryService inventoryService,
        OrderDbContext orderDbContext,
        IRabbitMqBroker broker,
        Action<string>? logger = null)
    {
        _inventoryService = inventoryService;
        _orderDbContext = orderDbContext;
        _broker = broker;
        _logger = logger;
    }

    /// <summary>
    /// Quy trình phối hợp thực chiến E-Commerce:
    /// 1. GỌI ĐỒNG BỘ (Sync Call): Gọi IInventoryService.CheckStockAsync để kiểm tra tồn kho tức thì.
    ///    - Nếu hết hàng -> Dừng ngay, trả lỗi OutOfStock (Demo 2).
    /// 2. GHI CSDL NỘI BỘ (Local ACID Transaction): Tạo đơn hàng trạng thái CONFIRMED.
    /// 3. BẮN SỰ KIỆN BẤT ĐỒNG BỘ (Async Event Publish): Bắn OrderCreatedEvent lên RabbitMQ Fanout Exchange (Demo 3).
    /// </summary>
    public async Task<CreateOrderResult> PlaceOrderAsync(CreateOrderRequest request)
    {
        _logger?.Invoke($"\n[OrderService] Bắt đầu xử lý yêu cầu tạo đơn từ Khách hàng: {request.CustomerId}");

        // =========================================================================
        // BƯỚC 1: GIAO TIẾP ĐỒNG BỘ (SYNCHRONOUS CALL) -> KIỂM TRA TỒN KHO
        // =========================================================================
        _logger?.Invoke(" -> [BƯỚC 1: ĐỒNG BỘ] Gọi IInventoryService.CheckStockAsync (gọi và chờ phản hồi tức thì)...");

        foreach (var item in request.Items)
        {
            var checkStockRequest = new CheckStockRequestDto(item.ProductId, item.Quantity);
            var stockResult = await _inventoryService.CheckStockAsync(checkStockRequest);

            if (!stockResult.IsAvailable)
            {
                _logger?.Invoke($" ❌ TỪ CHỐI TẠO ĐƠN! {stockResult.Message} (Sản phẩm: '{item.ProductName}')");

                // Lưu đơn ở trạng thái REJECTED để lưu vết kiểm toán
                var rejectedOrder = new Domain.Order(request.CustomerId);
                rejectedOrder.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
                rejectedOrder.Reject(stockResult.Message);
                _orderDbContext.Save(rejectedOrder);

                return new CreateOrderResult(
                    Success: false,
                    OrderId: rejectedOrder.Id,
                    Status: OrderStatus.Rejected,
                    TotalAmount: rejectedOrder.TotalAmount,
                    Message: $"Tạo đơn hàng THẤT BẠI do hết hàng: {stockResult.Message}"
                );
            }

            _logger?.Invoke($" ✔ Tồn kho hợp lệ: {stockResult.Message}");
        }

        // =========================================================================
        // BƯỚC 2: GHI CSDL ĐƠN HÀNG NỘI BỘ (LOCAL ACID TRANSACTION)
        // =========================================================================
        _logger?.Invoke(" -> [BƯỚC 2: LOCAL TRANSACTION] Ghi nhận đơn hàng vào CSDL Order (order_schema.json)...");
        var order = new Domain.Order(request.CustomerId);
        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }
        order.Confirm();
        _orderDbContext.Save(order);
        _logger?.Invoke($" ✔ Đã ghi thành công Đơn hàng #{order.Id} (Status: CONFIRMED | Tổng tiền: {order.TotalAmount:N0} VNĐ)");

        // =========================================================================
        // BƯỚC 3: GIAO TIẾP BẤT ĐỒNG BỘ (ASYNC EVENT PUBLISH QUA RABBITMQ)
        // =========================================================================
        _logger?.Invoke(" -> [BƯỚC 3: BẤT ĐỒNG BỘ] Phát sự kiện OrderCreatedEvent lên RabbitMQ Fanout Exchange 'ecommerce.order-events'...");

        var eventItems = order.Items.Select(i => new OrderItemEventDto(
            i.ProductId,
            i.ProductNameSnapshot,
            i.Quantity,
            i.UnitPriceSnapshot
        )).ToList();

        var orderCreatedEvent = new OrderCreatedEvent(
            EventId: Guid.NewGuid(),
            OrderId: order.Id,
            CustomerId: order.CustomerId,
            Items: eventItems,
            TotalAmount: order.TotalAmount,
            OccurredOn: DateTime.UtcNow,
            DeliveryMode: 2 // AMQP Disk Persistence
        );

        // Bắn sự kiện lên RabbitMQ Exchange (không đợi các consumer xử lý xong)
        await _broker.PublishAsync(
            exchangeName: "ecommerce.order-events",
            routingKey: "order.created",
            message: orderCreatedEvent,
            deliveryMode: 2
        );

        _logger?.Invoke(" ✔ Sự kiện OrderCreatedEvent đã được RabbitMQ ghi nhận và đưa vào hàng đợi an toàn!");

        // =========================================================================
        // BƯỚC 4: PHẢN HỒI TỨC THÌ CHO KHÁCH HÀNG (< 50ms)
        // =========================================================================
        return new CreateOrderResult(
            Success: true,
            OrderId: order.Id,
            Status: OrderStatus.Confirmed,
            TotalAmount: order.TotalAmount,
            Message: "Đặt hàng THÀNH CÔNG! Đơn hàng đã được xác nhận và chuyển tiếp xử lý ngầm."
        );
    }

    public IEnumerable<Domain.Order> GetAllOrders() => _orderDbContext.GetAll();
}
