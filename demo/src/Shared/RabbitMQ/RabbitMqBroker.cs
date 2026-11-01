using System.Collections.Concurrent;
using System.Text.Json;

namespace Shared.Kernel.RabbitMQ;

public class RabbitMqBroker : IRabbitMqBroker
{
    private class QueueDescriptor
    {
        public string Name { get; set; } = string.Empty;
        public bool Durable { get; set; }
        public ConcurrentQueue<string> Messages { get; set; } = new();
        public Func<string, Task<bool>>? ConsumerHandler { get; set; }
        public bool IsConsumerConnected { get; set; } = false;
        public bool AutoAck { get; set; } = false;
    }

    private class ExchangeDescriptor
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "fanout"; // fanout, direct, topic
        public bool Durable { get; set; }
        public List<(string QueueName, string RoutingKey)> Bindings { get; set; } = new();
    }

    private readonly ConcurrentDictionary<string, ExchangeDescriptor> _exchanges = new();
    private readonly ConcurrentDictionary<string, QueueDescriptor> _queues = new();
    private readonly Action<string>? _logger;

    public RabbitMqBroker(Action<string>? logger = null)
    {
        _logger = logger;
    }

    private void Log(string message)
    {
        _logger?.Invoke($"[RabbitMQ-Broker] {message}");
    }

    public void DeclareExchange(string exchangeName, string type = "fanout", bool durable = true)
    {
        _exchanges.GetOrAdd(exchangeName, name =>
        {
            Log($"Exchange declared: '{name}' | Type: {type.ToUpper()} | Durable: {durable}");
            return new ExchangeDescriptor { Name = name, Type = type.ToLower(), Durable = durable };
        });
    }

    public void DeclareQueue(string queueName, bool durable = true)
    {
        _queues.GetOrAdd(queueName, name =>
        {
            Log($"Queue declared: '{name}' | Durable: {durable}");
            return new QueueDescriptor { Name = name, Durable = durable };
        });
    }

    public void BindQueue(string queueName, string exchangeName, string routingKey = "")
    {
        if (!_exchanges.TryGetValue(exchangeName, out var exchange))
        {
            DeclareExchange(exchangeName, "fanout", true);
            exchange = _exchanges[exchangeName];
        }

        if (!_queues.ContainsKey(queueName))
        {
            DeclareQueue(queueName, true);
        }

        lock (exchange.Bindings)
        {
            if (!exchange.Bindings.Any(b => b.QueueName == queueName && b.RoutingKey == routingKey))
            {
                exchange.Bindings.Add((queueName, routingKey));
                Log($"Bound Queue '{queueName}' to Exchange '{exchangeName}' [RoutingKey: '{routingKey}']");
            }
        }
    }

    public async Task PublishAsync<TEvent>(string exchangeName, string routingKey, TEvent message, byte deliveryMode = 2)
    {
        if (!_exchanges.TryGetValue(exchangeName, out var exchange))
            throw new InvalidOperationException($"Exchange '{exchangeName}' does not exist.");

        var payload = JsonSerializer.Serialize(message);
        var eventType = typeof(TEvent).Name;

        Log($"[PUBLISH] Event '{eventType}' published to Exchange '{exchangeName}' | delivery_mode={deliveryMode} (Persistent)");

        List<string> targetQueues = new();

        lock (exchange.Bindings)
        {
            if (exchange.Type == "fanout")
            {
                // Fanout ignores routingKey and broadcasts to all bound queues
                targetQueues = exchange.Bindings.Select(b => b.QueueName).Distinct().ToList();
            }
            else if (exchange.Type == "direct")
            {
                targetQueues = exchange.Bindings
                    .Where(b => b.RoutingKey == routingKey)
                    .Select(b => b.QueueName).Distinct().ToList();
            }
            else if (exchange.Type == "topic")
            {
                targetQueues = exchange.Bindings
                    .Where(b => MatchTopic(b.RoutingKey, routingKey))
                    .Select(b => b.QueueName).Distinct().ToList();
            }
        }

        foreach (var qName in targetQueues)
        {
            if (_queues.TryGetValue(qName, out var queue))
            {
                queue.Messages.Enqueue(payload);
                Log($" -> Routed to Queue '{qName}' (Current depth: {queue.Messages.Count} messages)");
                _ = DispatchNextAsync(queue);
            }
        }

        await Task.CompletedTask;
    }

    private static bool MatchTopic(string bindingKey, string routingKey)
    {
        var bParts = bindingKey.Split('.');
        var rParts = routingKey.Split('.');
        // Simple wildcard match for * and #
        int b = 0, r = 0;
        while (b < bParts.Length && r < rParts.Length)
        {
            if (bParts[b] == "#") return true;
            if (bParts[b] == "*" || bParts[b] == rParts[r])
            {
                b++; r++;
            }
            else return false;
        }
        return b == bParts.Length && r == rParts.Length;
    }

    public void Subscribe<TEvent>(string queueName, Func<TEvent, Task<bool>> handler, bool autoAck = false)
    {
        if (!_queues.TryGetValue(queueName, out var queue))
            throw new InvalidOperationException($"Queue '{queueName}' does not exist.");

        queue.AutoAck = autoAck;
        queue.IsConsumerConnected = true;
        queue.ConsumerHandler = async (json) =>
        {
            var obj = JsonSerializer.Deserialize<TEvent>(json);
            if (obj == null) return false;
            return await handler(obj);
        };

        Log($"Consumer attached to Queue '{queueName}' | autoAck: {autoAck} | Status: ONLINE");
        _ = DispatchNextAsync(queue);
    }

    public void DisconnectConsumer(string queueName)
    {
        if (_queues.TryGetValue(queueName, out var queue))
        {
            queue.IsConsumerConnected = false;
            Log($"[OFFLINE EXPERIMENT] Consumer on Queue '{queueName}' has been DISCONNECTED! (Status: OFFLINE)");
        }
    }

    public void ReconnectConsumer(string queueName)
    {
        if (_queues.TryGetValue(queueName, out var queue))
        {
            queue.IsConsumerConnected = true;
            Log($"[OFFLINE EXPERIMENT] Consumer on Queue '{queueName}' has RECONNECTED! (Status: ONLINE - Pending messages: {queue.Messages.Count})");
            _ = DispatchNextAsync(queue);
        }
    }

    public int GetQueueMessageCount(string queueName)
    {
        return _queues.TryGetValue(queueName, out var queue) ? queue.Messages.Count : 0;
    }

    public bool IsConsumerOnline(string queueName)
    {
        return _queues.TryGetValue(queueName, out var queue) && queue.IsConsumerConnected;
    }

    private async Task DispatchNextAsync(QueueDescriptor queue)
    {
        if (!queue.IsConsumerConnected || queue.ConsumerHandler == null)
            return;

        while (queue.IsConsumerConnected && queue.Messages.TryDequeue(out var msg))
        {
            try
            {
                Log($"[DELIVER] Dispatching message from Queue '{queue.Name}' to Consumer...");
                var ack = await queue.ConsumerHandler(msg);

                if (ack || queue.AutoAck)
                {
                    Log($"[ACK RECEIVED] BasicAck confirmed for message on Queue '{queue.Name}'.");
                }
                else
                {
                    Log($"[NACK RECEIVED] BasicNack received, requeuing message on Queue '{queue.Name}'!");
                    queue.Messages.Enqueue(msg);
                    break;
                }
            }
            catch (Exception ex)
            {
                Log($"[CONSUMER EXCEPTION] Error in consumer for '{queue.Name}': {ex.Message}. Requeuing message!");
                queue.Messages.Enqueue(msg);
                break;
            }
        }
    }
}
