namespace BuildingBlocks.Messaging.Messaging.Kafka;

public interface IKafkaProducerService<TKey, TValue>
{
    Task ProduceAsync(string topic, TKey key, TValue value);
}