namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class EventBase : IEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
}