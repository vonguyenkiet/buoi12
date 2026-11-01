using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Inventory.Application;
using Modules.Inventory.Domain;
using Modules.Inventory.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Inventory.Consumers;

public class InventoryOrderCreatedConsumer : BackgroundService
{
    private readonly IRabbitMqConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<InventoryOrderCreatedConsumer> _logger;
    private IChannel? _channel;

    public InventoryOrderCreatedConsumer(
        IRabbitMqConnection connection,
        IServiceScopeFactory scopeFactory,
        ILogger<InventoryOrderCreatedConsumer> logger)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            ulong deliveryTag = ea.DeliveryTag;

            try
            {
                var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(json);
                if (@event == null)
                {
                    _logger.LogWarning("[INVENTORY-CONSUMER] Payload sự kiện null, gửi basicAck để loại bỏ.");
                    await _channel.BasicAckAsync(deliveryTag, multiple: false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
                var service = scope.ServiceProvider.GetRequiredService<InProcessInventoryService>();

                // Kiểm tra tính Idempotent (Chống xử lý trùng lặp sự kiện)
                var alreadyProcessed = await dbContext.ProcessedEvents.AnyAsync(p => p.EventId == @event.EventId);
                if (alreadyProcessed)
                {
                    _logger.LogInformation(
                        "[INVENTORY-CONSUMER] [IDEMPOTENT] Sự kiện {EventId} đã được xử lý trước đó. Bỏ qua và gửi basicAck.",
                        @event.EventId);
                    await _channel.BasicAckAsync(deliveryTag, multiple: false);
                    return;
                }

                _logger.LogInformation(
                    "[INVENTORY-CONSUMER] [RABBITMQ] Nhận sự kiện OrderCreated #{OrderId} (Event: {EventId}) từ queue '{Queue}'. Đang cập nhật tồn kho (Reserved -> Sold)...",
                    @event.OrderId, @event.EventId, RabbitMqConstants.InventoryOrderCreatedQueue);

                // Chuyển hàng từ Reserved sang Sold
                foreach (var item in @event.Items)
                {
                    await service.ConfirmSoldAsync(item.ProductId, item.Quantity);
                }

                // Ghi nhận EventId đã xử lý
                dbContext.ProcessedEvents.Add(new InventoryProcessedEvent
                {
                    EventId = @event.EventId,
                    ProcessedAt = DateTime.UtcNow
                });
                await dbContext.SaveChangesAsync();

                // Gửi Manual ACK
                await _channel.BasicAckAsync(deliveryTag, multiple: false);
                _logger.LogInformation(
                    "[INVENTORY-CONSUMER] [ACK] Đã hoàn tất trừ kho và gửi basicAck cho đơn hàng #{OrderId}.",
                    @event.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[INVENTORY-CONSUMER] [ERROR] Lỗi khi xử lý sự kiện OrderCreated: {Message}", ex.Message);

                // Kiểm tra retry count qua headers
                int retryCount = 0;
                if (ea.BasicProperties.Headers != null &&
                    ea.BasicProperties.Headers.TryGetValue(RabbitMqConstants.HeaderRetryCount, out var val))
                {
                    if (val is byte[] bytes && int.TryParse(Encoding.UTF8.GetString(bytes), out var count))
                        retryCount = count;
                }

                retryCount++;
                if (retryCount <= RabbitMqConstants.MaxRetryCount)
                {
                    _logger.LogWarning(
                        "[INVENTORY-CONSUMER] [NACK-REQUEUE] Gửi basicNack requeue (Lần thử {Retry}/{Max}).",
                        retryCount, RabbitMqConstants.MaxRetryCount);
                    await _channel.BasicNackAsync(deliveryTag, multiple: false, requeue: true);
                }
                else
                {
                    _logger.LogError(
                        "[INVENTORY-CONSUMER] [NACK-DEADLETTER] Quá {Max} lần thử thất bại! Chuyển tin nhắn vào Dead-Letter Queue.",
                        RabbitMqConstants.MaxRetryCount);
                    await _channel.BasicNackAsync(deliveryTag, multiple: false, requeue: false);
                }
            }
        };

        await _channel.BasicConsumeAsync(
            queue: RabbitMqConstants.InventoryOrderCreatedQueue,
            autoAck: false, // Bắt buộc Manual ACK
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("[INVENTORY-CONSUMER] Đã khởi động lắng nghe queue '{Queue}' (Manual ACK).", RabbitMqConstants.InventoryOrderCreatedQueue);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken);
            _channel.Dispose();
        }
        await base.StopAsync(cancellationToken);
    }
}
