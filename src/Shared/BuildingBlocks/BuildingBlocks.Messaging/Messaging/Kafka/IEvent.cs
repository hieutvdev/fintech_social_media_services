namespace BuildingBlocks.Messaging.Messaging.Kafka;

public interface IEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}
