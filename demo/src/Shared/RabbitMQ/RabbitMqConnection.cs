using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Shared.Kernel.RabbitMQ;

public class RabbitMqConnection : IRabbitMqConnection
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMqConnection> _logger;
    private IConnection? _connection;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMqConnection(
        string host,
        int port,
        string username,
        string password,
        ILogger<RabbitMqConnection> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
            UserName = username,
            Password = password
        };
    }

    private async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection != null && _connection.IsOpen)
            return _connection;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _logger.LogInformation("[RABBITMQ] Đang kết nối tới RabbitMQ Broker tại {Host}:{Port}...", _factory.HostName, _factory.Port);
                _connection = await _factory.CreateConnectionAsync(cancellationToken);
                _logger.LogInformation("[RABBITMQ] ✔ Đã kết nối thành công tới RabbitMQ Broker.");
            }
            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IChannel> CreateChannelAsync(CreateChannelOptions? options = null, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken);
        return options != null
            ? await conn.CreateChannelAsync(options, cancellationToken)
            : await conn.CreateChannelAsync(cancellationToken: cancellationToken);
    }

    public async Task InitializeTopologyAsync(CancellationToken cancellationToken = default)
    {
        var channel = await CreateChannelAsync(cancellationToken: cancellationToken);
        try
        {
            _logger.LogInformation("[RABBITMQ] Khởi tạo Topology: Exchange 'order.events', Dead-letter Exchange 'order.dlx' và các Queues durable...");

            // 1. Dead Letter Exchange & Queue
            await channel.ExchangeDeclareAsync(
                exchange: RabbitMqConstants.DeadLetterExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.DeadLetterQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.DeadLetterQueue,
                exchange: RabbitMqConstants.DeadLetterExchange,
                routingKey: RabbitMqConstants.DeadLetterRoutingKey,
                cancellationToken: cancellationToken);

            // 2. Primary Fanout Exchange
            await channel.ExchangeDeclareAsync(
                exchange: RabbitMqConstants.OrderEventsExchange,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            var queueArgs = new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = RabbitMqConstants.DeadLetterExchange,
                ["x-dead-letter-routing-key"] = RabbitMqConstants.DeadLetterRoutingKey
            };

            // 3. Inventory Queue
            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.InventoryOrderCreatedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: queueArgs,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.InventoryOrderCreatedQueue,
                exchange: RabbitMqConstants.OrderEventsExchange,
                routingKey: string.Empty,
                cancellationToken: cancellationToken);

            // 4. Notification Queue
            await channel.QueueDeclareAsync(
                queue: RabbitMqConstants.NotificationOrderCreatedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: queueArgs,
                cancellationToken: cancellationToken);

            await channel.QueueBindAsync(
                queue: RabbitMqConstants.NotificationOrderCreatedQueue,
                exchange: RabbitMqConstants.OrderEventsExchange,
                routingKey: string.Empty,
                cancellationToken: cancellationToken);

            _logger.LogInformation("[RABBITMQ] ✔ Topology đã sẵn sàng: Exchange, Queues durable và DLQ đã được ánh xạ.");
        }
        finally
        {
            await channel.CloseAsync(cancellationToken);
        }
    }

    public async Task PurgeQueueAsync(string queueName, CancellationToken cancellationToken = default)
    {
        var channel = await CreateChannelAsync(cancellationToken: cancellationToken);
        try
        {
            await channel.QueuePurgeAsync(queueName, cancellationToken);
            _logger.LogInformation("[RABBITMQ] Đã xóa rỗng (purge) hàng đợi '{QueueName}'.", queueName);
        }
        finally
        {
            await channel.CloseAsync(cancellationToken);
        }
    }

    public async Task<uint> GetMessageCountAsync(string queueName, CancellationToken cancellationToken = default)
    {
        var channel = await CreateChannelAsync(cancellationToken: cancellationToken);
        try
        {
            var declareOk = await channel.QueueDeclarePassiveAsync(queueName, cancellationToken);
            return declareOk.MessageCount;
        }
        finally
        {
            await channel.CloseAsync(cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
        _lock.Dispose();
    }
}
