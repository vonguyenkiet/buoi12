namespace Shared.Kernel.RabbitMQ;

public interface IRabbitMqBroker
{
    void DeclareExchange(string exchangeName, string type = "fanout", bool durable = true);
    void DeclareQueue(string queueName, bool durable = true);
    void BindQueue(string queueName, string exchangeName, string routingKey = "");
    Task PublishAsync<TEvent>(string exchangeName, string routingKey, TEvent message, byte deliveryMode = 2);
    void Subscribe<TEvent>(string queueName, Func<TEvent, Task<bool>> handler, bool autoAck = false);
    void DisconnectConsumer(string queueName);
    void ReconnectConsumer(string queueName);
    int GetQueueMessageCount(string queueName);
    bool IsConsumerOnline(string queueName);
}
