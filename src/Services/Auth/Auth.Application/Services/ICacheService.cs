namespace Auth.Application.Services;

public interface ICacheService
{
    Task SetCacheAsync(string cacheKey, object? value, TimeSpan expire);
    Task<string> GetCacheAsync(string cacheKey);
    Task RemoveCacheAsync(string cacheKey);

    Task SetStringAsync(string cacheKey, string value, TimeSpan expire);
    Task<string> GetStringAsync(string cacheKey);

    Task SetStringIncrementAsync(string cacheKey);
    Task SetExpireForKeyAsync(string cacheKey, TimeSpan expire);

    Task<int> GetIncrementValueAsync(string cacheKey);

    Task<TimeSpan?> GetKeyTimeToLiveAsync(string cacheKey);

    Task<IEnumerable<string>> GetKeyStringsAsync(string parentKey);
}