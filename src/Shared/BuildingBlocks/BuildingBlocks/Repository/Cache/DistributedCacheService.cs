using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BuildingBlocks.Repository.Cache;

public class DistributedCacheService : IDistributedCacheService
{

    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public DistributedCacheService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
    {
        _cache = cache;
        _connectionMultiplexer = connectionMultiplexer;
    }
    
    public Task SetCacheAsync(string cacheKey, object? value, TimeSpan expire)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetCacheAsync(string cacheKey)
    {
        var cacheResponse = await _cache.GetStringAsync(cacheKey);

        return string.IsNullOrWhiteSpace(cacheResponse) ? "" : cacheResponse;
    }

    public async Task RemoveCacheAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        await foreach (var key in GetKeyStartWithAsync(cacheKey))
        {
           await _cache.RemoveAsync(key);
        }
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
            throw new NotImplementedException();
        }

        var db = _connectionMultiplexer.GetDatabase();
        var keyType = await db.KeyTypeAsync(cacheKey);
        if (keyType != RedisType.String)
        {
            throw new InvalidOperationException($"Key '{cacheKey}' does not hold a string value.");
        }

        string cacheResponse = (await db.StringGetAsync(cacheKey))!;
        return string.IsNullOrEmpty(cacheResponse) ? "" : cacheResponse;
    }

    public async Task SetStringIncrementAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();
        var keyType = await db.KeyTypeAsync(cacheKey);
        if (keyType != RedisType.String)
        {
            throw new InvalidOperationException($"Key '{cacheKey}' does not hold a string value.");
        }

        await db.StringIncrementAsync(cacheKey);
    }

    public async Task SetExpireForKeyAsync(string cacheKey, TimeSpan expire)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Cache key cannot be null or empty");
        }

        if (expire < TimeSpan.FromSeconds(1))
        {
            throw new BadRequestException("Invalid timespan");
        }

        var db = _connectionMultiplexer.GetDatabase();
        var keyExists = await KeyExistAsync(cacheKey);
        if (!keyExists)
        {
            throw new BadRequestException("Key is not exist");
        }

        await db.KeyExpireAsync(cacheKey, expire);

    }

    public async Task<int> GetIncrementValueAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Cache key cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();

        var value = (await db.StringGetAsync(cacheKey))!;
        return value.HasValue ? (int)value : 0;
    }

    public async Task<TimeSpan?> GetKeyTimeToLiveAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Cache key cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();
        if (!KeyExistAsync(cacheKey).GetAwaiter().GetResult())
        {
            throw new BadRequestException("Key is not exist");
        }

        var timeToLive = await db.KeyTimeToLiveAsync(cacheKey);
        return timeToLive;
    }

    public async Task<IEnumerable<string>> GetKeyStringsAsync(string parentKey)
    {
        await Task.Yield();
        if (string.IsNullOrEmpty(parentKey))
        {
            throw new ArgumentNullException($"Value cannot be null or empty {nameof(GetKeyStringsAsync)}");
        }

        IList<string> keys = [];
        foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);
            foreach (var key in server.Keys(pattern: parentKey + "*"))
            {
                keys.Add(key!);
            }
        }

        return keys.Any() ? keys : Enumerable.Empty<string>();
    }


    private async IAsyncEnumerable<string> GetKeyStartWithAsync(string parentKey)
    {
        await Task.Yield();
        if (string.IsNullOrWhiteSpace(parentKey))
        {
            throw new ArgumentException("Cache key cannot be null or whitespace");
        }

        IList<string> keys = [];
        foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);

            foreach (var key in server.Keys(pattern: parentKey +"*"))
            {
                yield return key.ToString();
            }
        }
    }


    private async Task<bool> KeyExistAsync(string keyCache)
    {
        if (string.IsNullOrEmpty(keyCache))
        {
            throw new ArgumentException("Value cannot be null or empty");
        }

        var db = _connectionMultiplexer.GetDatabase();

        return await db.KeyExistsAsync(keyCache);

    }
}