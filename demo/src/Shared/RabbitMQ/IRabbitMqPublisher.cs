namespace Shared.Kernel.RabbitMQ;

public interface IRabbitMqPublisher
{
    Task PublishAsync<TEvent>(string exchange, TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
}
