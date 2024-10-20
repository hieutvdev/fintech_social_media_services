namespace BuildingBlocks.Repository.Cache;


#nullable disable
public class CacheSetting
{
    public string CacheType { get; set; }
    
    public RedisCacheSetting RedisCacheSetting { get; set; }
    
    public DbCacheSetting DbCacheSetting { get; set; }
}