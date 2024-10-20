namespace BuildingBlocks.Repository.Cache;


#nullable disable
public class RedisCacheSetting
{
    public bool Enabled { get; set; } = true;
    public string Connection { get; set; }
    
    public string InstanceName  { get; set; }
}