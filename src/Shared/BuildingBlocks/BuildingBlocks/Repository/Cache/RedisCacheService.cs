using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace BuildingBlocks.Repository.Cache;

public class RedisCacheService(IDistributedCache cache, IConnectionMultiplexer connection) : ICacheService
{
    public async Task SetAsync(string cacheKey, object? value, TimeSpan? expiration = null)
    {
        if (value is null)
        {
            return;
        }
        var objectSerialized = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        await cache.SetStringAsync(cacheKey, objectSerialized, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });

    }

    public async Task<string> GetAsync(string cacheKey)
    {
        var cacheResponse = await cache.GetStringAsync(cacheKey);
        return string.IsNullOrEmpty(cacheResponse) ? "" : cacheResponse;
    }

    public async Task RemoveAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or whitespace");
        }

        await foreach (string key in GetAllKeyStartWithAsync(cacheKey))
        {
            await cache.RemoveAsync(key);
        }
    }



    private async IAsyncEnumerable<string> GetAllKeyStartWithAsync(string startWith)
    {
        await Task.Yield();
        if (string.IsNullOrEmpty(startWith))
        {
            throw new ArgumentException("Value cannot be null or whitespace");
        }

        foreach (var endPoint in connection.GetEndPoints())
        {
            var server = connection.GetServer(endPoint);
            foreach (var redisKey in server.Keys(pattern: startWith + "*"))
            {
                yield return redisKey.ToString();
            }
        }
    }
}