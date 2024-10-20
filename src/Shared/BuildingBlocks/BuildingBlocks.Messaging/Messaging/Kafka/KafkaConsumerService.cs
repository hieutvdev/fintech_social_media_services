using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class KafkaConsumerService<TKey, TValue> : IKafkaConsumerService<TKey, TValue>, IDisposable
{
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaConsumerService<TKey, TValue>> _logger;
    
    public KafkaConsumerService(IOptions<KafkaSettings> options, ILogger<KafkaConsumerService<TKey, TValue>> logger)
    {
        _logger = logger;
        _kafkaSettings = options.Value;
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<TKey, TValue>(config).Build();
    }
    
    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
    }

    public async Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage)
    {
        while (true)
        {
            
            
            var consumerResult = _consumer.Consume();
            _logger.LogInformation($"Consumer -------------- {consumerResult.Message.Key}");
            if (consumerResult != null)
            {
                await handleMessage(consumerResult.Message.Key, consumerResult.Message.Value);
            }
        }
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}