using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BuildingBlocks.Repository.Cache;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    ConcurrentDictionary<string, bool> _key = new ();
    public Task SetAsync(string cacheKey, object? value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            SlidingExpiration = expiration ?? TimeSpan.FromHours(5)
        };
        cache.Set(cache, value, options);
        _key.TryAdd(cacheKey, true);
        return Task.CompletedTask;
    }

    public Task<string> GetAsync(string cacheKey)
    {
        cache.TryGetValue(cacheKey, out object? value);
        var objectSerialized = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        return Task.FromResult(objectSerialized);
    }

    public Task RemoveAsync(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
        {
            throw new ArgumentException("Value cannot be null or whitespace.");
        }

        foreach (var key in GetKeysStartWith(cacheKey))
        {
            cache.Remove(key);
            _key.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }


    private IEnumerable<string> GetKeysStartWith(string prefix)
    {
        return _key.Keys.Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}