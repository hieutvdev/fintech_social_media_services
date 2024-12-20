

using Auth.Application.Services;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace Auth.Infrastructure.Services;


public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CacheService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
    {
        _cache = cache;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string> GetCacheAsync(string cacheKey)
    {
        var cacheResponse = await _cache.GetStringAsync(cacheKey);

        return string.IsNullOrWhiteSpace(cacheResponse) ? "" : cacheResponse;
    }

    public async Task RemoveCacheAsync(string cacheKey)
    {
        if (string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or whitespace");
        }
        await foreach (var key in GetKeyStartWithAsync(cacheKey)) {
            await _cache.RemoveAsync(key);
        }
    }

    public async Task SetStringIncrementAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();
        await db.StringIncrementAsync(cacheKey);
    }

    public async Task SetExpireForKeyAsync(string cacheKey, TimeSpan expire)
    {
        if (string.IsNullOrWhiteSpace(cacheKey) || expire < TimeSpan.FromSeconds(1))
        {
            throw new BadRequestException("Invalid value");
        }

        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyExpireAsync(cacheKey, expire);
    }

    public async Task<int> GetIncrementValueAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();
        var value = (await db.StringGetAsync(cacheKey))!;

        return value.HasValue ? (int)value : 0;
    }

    public async Task<TimeSpan?> GetKeyTimeToLiveAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();
        var timeToLive = await db.KeyTimeToLiveAsync(cacheKey);
        return timeToLive;
    }

  

    public async Task SetCacheAsync(string cacheKey, object? value, TimeSpan expire)
    {
        if(value is null)
        {
            return;
        }

        var serializedRequest = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        });

        await _cache.SetStringAsync(cacheKey, serializedRequest, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expire
        });
    }



    private async IAsyncEnumerable<string> GetKeyStartWithAsync(string value)
    {
        await Task.Yield();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace");
        }

        foreach(var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);
            foreach(var key in server.Keys(pattern: value + "*"))
            {
                yield return key.ToString();
            }
        }
    }
    public async Task<IEnumerable<string>> GetKeyStringsAsync(string parentKey)
    {
        
        if (string.IsNullOrEmpty(parentKey))
        {
            throw new ArgumentNullException($"Value cannot be null or empty {nameof(GetKeyStringsAsync)}");
        }

        IList<string> keys = [];
        foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);
            foreach (var redisKey in server.Keys(pattern: parentKey +"*"))
            {
                keys.Add(redisKey!);
            }
        }

        return keys.AsEnumerable();


    }

    public async Task SetStringAsync(string cacheKey, string value, TimeSpan expire)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace");
        }
        await _cache.SetStringAsync(cacheKey, value, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expire
        });
    }

    public async Task<string> GetStringAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentNullException($"Value cannot be null or empty {nameof(CacheService)}");
        }
        var db = _connectionMultiplexer.GetDatabase();
        var keyType = await db.KeyTypeAsync(cacheKey);
        if (keyType != RedisType.String)
        {
            throw new InvalidOperationException($"Key '{cacheKey}' does not hold a string value.");
        }
        string cacheResponse = await db.StringGetAsync(cacheKey);
        return cacheResponse;
    }
}
