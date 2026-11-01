using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Shared.Kernel.RabbitMQ;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IRabbitMqConnection _connection;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IRabbitMqConnection connection, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(string exchange, TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        var options = new CreateChannelOptions(
            publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true);

        var channel = await _connection.CreateChannelAsync(options, cancellationToken);
        try
        {
            var eventType = typeof(TEvent).Name;
            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent, // AMQP Persistent delivery_mode = 2 (Lưu trên đĩa)
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString(),
                Type = eventType,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: string.Empty,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "[RABBITMQ] [PUBLISH] Sự kiện '{EventType}' đã publish lên Exchange '{Exchange}' | delivery_mode=2 (Persistent) | Publisher Confirmed.",
                eventType, exchange);
        }
        finally
        {
            await channel.CloseAsync(cancellationToken);
        }
    }
}
