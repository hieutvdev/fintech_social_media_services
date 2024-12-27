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
            BootstrapServers = _kafkaSettings.BootstrapServers,
            MessageTimeoutMs = 5000,
            EnableIdempotence = true,
            Acks = Acks.All
        };
        _producer = new ProducerBuilder<TKey, TValue>(config).SetValueSerializer(new CustomSerializer<TValue>()).Build();
    }
    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        int retryCount = 0;

        while (retryCount <= _kafkaSettings.RetryCount)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow.AddHours(7)} PUBLISH MESSAGE WITH TOPIC {topic}");
                await _producer.ProduceAsync(topic, new Message<TKey, TValue> { Key = key, Value = value});
                _logger.LogInformation($"{DateTime.UtcNow.AddHours(7)} SUCCESSFULLY PUBLISHED MESSAGE TO TOPIC {topic}");
                break;
            }
            catch (Exception e)
            { 
                retryCount++;
                _logger.LogError($"{DateTime.UtcNow.AddHours(7)} ERROR WHILE PUBLISHING TO TOPIC {topic}: {e.Message}");

                if (retryCount > _kafkaSettings.RetryCount)
                {
                    _logger.LogError($"{DateTime.UtcNow.AddHours(7)} MAX RETRY REACHED. SENDING TO ERROR TOPIC {_kafkaSettings.ErrorTopic}");
                    await SendToErrorTopicAsync(topic, key, value, e.Message);
                    break;
                }

                await Task.Delay(_kafkaSettings.RetryIntervalMs);
            

            }
        }
        
    }
    
    /// <summary>
    /// Produce multiple messages in batch.
    /// </summary>
    public async Task ProduceBatchAsync(string topic, IEnumerable<(TKey Key, TValue Value)> messages)
    {
        var messageBatch = new List<Task>();

        foreach (var (key, value) in messages)
        {
            messageBatch.Add(ProduceAsync(topic, key, value));
        }

        await Task.WhenAll(messageBatch);
        _logger.LogInformation($"Batch of messages published to topic {topic} successfully.");
    }
    
    /// <summary>
    /// Send failed messages to the error topic.
    /// </summary>
    private async Task SendToErrorTopicAsync(string originalTopic, TKey key, TValue value, string errorMessage)
    {
        var errorPayload = new
        {
            OriginalTopic = originalTopic,
            Key = key,
            Value = value,
            ErrorMessage = errorMessage,
            Timestamp = DateTime.UtcNow
        };

        string errorTopic = _kafkaSettings.ErrorTopic;
        string serializedError = JsonConvert.SerializeObject(errorPayload);

        await _producer.ProduceAsync(errorTopic, new Message<TKey, TValue>
        {
            Key = key,
            Value = (TValue)(object)serializedError // Convert error payload to TValue
        });

        _logger.LogWarning($"Message sent to error topic: {errorTopic}");
    }


    public void Dispose()
    {
        _producer.Dispose();
    }
}