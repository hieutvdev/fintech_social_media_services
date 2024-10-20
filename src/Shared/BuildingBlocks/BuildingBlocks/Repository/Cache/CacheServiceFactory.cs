using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Repository.Cache;

public class CacheServiceFactory
{
    private readonly IServiceProvider _service;
    private readonly CacheSetting _cacheSetting;

    public CacheServiceFactory(IServiceProvider service, IOptions<CacheSetting> options)
    {
        _service = service;
        _cacheSetting = options.Value;
    }

    public ICacheService CreateCacheService()
    {
        var cacheType = _cacheSetting.CacheType;

        return cacheType switch
        {
            nameof(CacheOptions.Redis) => _service.GetRequiredService<RedisCacheService>(),
            nameof(CacheOptions.Memory) => _service.GetRequiredService<MemoryCacheService>(),
            nameof(CacheOptions.Database) => _service.GetRequiredService<DbCacheService>(),
            _ => throw new AggregateException("Invalid cache type configuration")
        };
    }
}