namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class KafkaSettings
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    public Dictionary<string, string> Topics { get; set; }
}