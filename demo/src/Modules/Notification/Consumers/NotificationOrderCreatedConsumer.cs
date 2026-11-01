using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Notification.Domain;
using Modules.Notification.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Kernel.Events;
using Shared.Kernel.RabbitMQ;

namespace Modules.Notification.Consumers;

public class NotificationOrderCreatedConsumer : BackgroundService
{
    private readonly IRabbitMqConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationOrderCreatedConsumer> _logger;
    private IChannel? _channel;
    private string? _consumerTag;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _isPaused = false;

    public bool IsActive => _channel != null && _channel.IsOpen && !string.IsNullOrEmpty(_consumerTag) && !_isPaused;

    public NotificationOrderCreatedConsumer(
        IRabbitMqConnection connection,
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationOrderCreatedConsumer> logger)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartConsumingInternalAsync(stoppingToken);
    }

    private async Task StartConsumingInternalAsync(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_channel == null || !_channel.IsOpen)
            {
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: cancellationToken);
            }

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
                        await _channel.BasicAckAsync(deliveryTag, multiple: false);
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                    // Kiểm tra tính Idempotent (Chống gửi thông báo trùng lặp)
                    var alreadyProcessed = await db.ProcessedEvents.AnyAsync(p => p.EventId == @event.EventId);
                    if (alreadyProcessed)
                    {
                        _logger.LogInformation(
                            "[NOTIFICATION] [IDEMPOTENT] Sự kiện {EventId} đã được gửi thông báo trước đó. Bỏ qua và gửi basicAck.",
                            @event.EventId);
                        await _channel.BasicAckAsync(deliveryTag, multiple: false);
                        return;
                    }

                    _logger.LogInformation(
                        "[NOTIFICATION] [RABBITMQ] Nhận sự kiện OrderCreated #{OrderId} (Event: {EventId}) từ queue '{Queue}'. Đang gửi thông báo...",
                        @event.OrderId, @event.EventId, RabbitMqConstants.NotificationOrderCreatedQueue);

                    // Gửi email giả lập (ghi rõ là mô phỏng trong log & DB)
                    var recipient = $"customer_{@event.CustomerId.ToString()[..6]}@example.com";
                    var message = $"[MÔ PHỎNG] Đơn hàng #{Path.GetFileNameWithoutExtension(@event.OrderId.ToString()[..8])} tổng tiền {@event.TotalAmount:N0} VNĐ đã được tiếp nhận thành công!";

                    var log = new NotificationLog
                    {
                        OrderId = @event.OrderId,
                        EventId = @event.EventId,
                        Channel = "EMAIL",
                        Recipient = recipient,
                        Message = message,
                        IsSimulation = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    db.NotificationLogs.Add(log);
                    db.ProcessedEvents.Add(new NotificationProcessedEvent
                    {
                        EventId = @event.EventId,
                        ProcessedAt = DateTime.UtcNow
                    });
                    await db.SaveChangesAsync();

                    _logger.LogInformation(
                        "[NOTIFICATION] [EMAIL-SIMULATED] (Mô phỏng) Đã gửi Email xác nhận tới {Recipient} cho đơn #{OrderId}.",
                        recipient, @event.OrderId);

                    // Gửi Manual ACK
                    await _channel.BasicAckAsync(deliveryTag, multiple: false);
                    _logger.LogInformation(
                        "[NOTIFICATION] [ACK] Đã hoàn tất ghi log thông báo và gửi basicAck cho đơn hàng #{OrderId}.",
                        @event.OrderId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[NOTIFICATION] [ERROR] Lỗi khi xử lý thông báo đơn hàng: {Message}", ex.Message);
                    await _channel.BasicNackAsync(deliveryTag, multiple: false, requeue: true);
                }
            };

            _consumerTag = await _channel.BasicConsumeAsync(
                queue: RabbitMqConstants.NotificationOrderCreatedQueue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            _isPaused = false;
            _logger.LogInformation(
                "[NOTIFICATION] ✔ Consumer đã BẬT và đang lắng nghe queue '{Queue}' (Tag: {Tag}).",
                RabbitMqConstants.NotificationOrderCreatedQueue, _consumerTag);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task StopConsumerAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_channel != null && _channel.IsOpen && !string.IsNullOrEmpty(_consumerTag))
            {
                await _channel.BasicCancelAsync(_consumerTag, cancellationToken: cancellationToken);
                _consumerTag = null;
                _isPaused = true;
                _logger.LogWarning(
                    "[NOTIFICATION] [OFFLINE-DEMO] Consumer Notification đã TẮT (BasicCancel). Tin nhắn mới sẽ NẰM CHỜ TRONG QUEUE (Ready=1)!");
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task StartConsumerAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[NOTIFICATION] [ONLINE-DEMO] Đang bật lại Consumer Notification để tiêu thụ tin nhắn tồn đọng...");
        await StartConsumingInternalAsync(cancellationToken);
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
