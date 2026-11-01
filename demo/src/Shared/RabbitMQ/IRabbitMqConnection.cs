using RabbitMQ.Client;

namespace Shared.Kernel.RabbitMQ;

public interface IRabbitMqConnection : IAsyncDisposable
{
    Task<IChannel> CreateChannelAsync(CreateChannelOptions? options = null, CancellationToken cancellationToken = default);
    Task InitializeTopologyAsync(CancellationToken cancellationToken = default);
    Task PurgeQueueAsync(string queueName, CancellationToken cancellationToken = default);
    Task<uint> GetMessageCountAsync(string queueName, CancellationToken cancellationToken = default);
}
