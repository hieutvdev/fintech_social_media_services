namespace ShredKernel.BaseClasses.Configurations;

public class KafkaConfiguration
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int RetryCount { get; set; } = 5; 
    public int RetryIntervalMs { get; set; } = 1000; 
    public string ErrorTopic { get; set; } = "error_topic"; 
}