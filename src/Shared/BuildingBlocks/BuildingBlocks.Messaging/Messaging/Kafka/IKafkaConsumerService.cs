using Confluent.Kafka;

namespace BuildingBlocks.Messaging.Messaging.Kafka;

// public interface IKafkaConsumerService<TKey, TValue>
// {
//     void Subscribe(string topic);
//     Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage);
// }

public interface IKafkaConsumerService<TKey, TValue>
{
    void Subscribe(string topic);
    Task ConsumeAsync(Func<TKey, TValue, Task> handleMessage);
    Task ConsumeBatchAsync(Func<IEnumerable<ConsumeResult<TKey, TValue>>, Task> handleBatchMessage);
}