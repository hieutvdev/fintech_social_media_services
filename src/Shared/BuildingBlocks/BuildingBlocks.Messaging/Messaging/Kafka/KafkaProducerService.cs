using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class KafkaProducerService<TKey, TValue> : IKafkaProducerService<TKey, TValue>, IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducerService<TKey, TValue>> _logger;
    private readonly KafkaSettings _kafkaSettings;

    public KafkaProducerService(IOptions<KafkaSettings> options, ILogger<KafkaProducerService<TKey, TValue>> logger)
    {
        _logger = logger;
        _kafkaSettings = options.Value;
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers
        };
        _producer = new ProducerBuilder<TKey, TValue>(config).Build();
    }
    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        try
        {
            _logger.LogInformation($"{DateTime.UtcNow.AddHours(7)} PUBLISH MESSAGE WITH TOPIC {topic}");
            await _producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value});
        }
        catch (Exception e)
        { 
            _logger.LogError($"{DateTime.UtcNow.AddHours(7)} FAILURE TO PUBLISH MESSAGE WITH TOPIC {topic}");
            _logger.LogError($"{DateTime.UtcNow.AddHours(7)} PUBLISH ERROR {e.Message}");
            

        }
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}