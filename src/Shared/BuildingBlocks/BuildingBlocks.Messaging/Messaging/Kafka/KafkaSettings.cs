namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class KafkaSettings
{
    public string BootstrapServers { get; set; }
    public string GroupId { get; set; }
    
    public Dictionary<string, string> Topics { get; set; }
    
    public int RetryCount { get; set; } = 5; 
    
    public int RetryIntervalMs { get; set; } = 1000; 
    
    public string ErrorTopic { get; set; } = "error_topic"; 
}

