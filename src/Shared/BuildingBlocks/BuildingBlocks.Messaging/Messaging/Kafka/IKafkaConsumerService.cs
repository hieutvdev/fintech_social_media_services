namespace BuildingBlocks.Messaging.Messaging.Kafka;

public interface IKafkaConsumerService<TKey, TValue>
{
    void Subscribe(string topic);
    Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage);
}