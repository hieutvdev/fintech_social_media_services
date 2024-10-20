namespace ShredKernel.BaseClasses.Configurations;


#nullable disable
public class RedisConfiguration
{
    public bool Enabled { get; set; }
    public string Host { get; set; }
    public string Port { get; set; }
}