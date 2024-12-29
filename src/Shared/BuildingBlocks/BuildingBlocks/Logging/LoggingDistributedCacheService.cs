using BuildingBlocks.Repository.Cache;


namespace BuildingBlocks.Logging;

public class LoggingDistributedCacheService : IDistributedCacheService
{
    private readonly ILoggingExtension<LoggingDistributedCacheService> _loggingExtension;
    private readonly IDistributedCacheService _distributedCache;

    public LoggingDistributedCacheService(
        ILoggingExtension<LoggingDistributedCacheService> loggingExtension,
        IDistributedCacheService distributedCacheService)
    {
        _loggingExtension = loggingExtension;
        _distributedCache = distributedCacheService;
    }

    public async Task SetCacheAsync(string cacheKey, object? value, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET CACHE VALUE {nameof(value)} TO {cacheKey} EXPIRATION {expire}");
            await _distributedCache.SetCacheAsync(cacheKey, value, expire);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET CACHE ERROR: {e.Message}");
            throw;
        }
    }

    public async Task<string> GetCacheAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET CACHE VALUE {cacheKey}");
            return await _distributedCache.GetCacheAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET CACHE ERROR: {e.Message}");
            throw;
        }
    }

    public async Task RemoveCacheAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] REMOVE CACHE {cacheKey}");
            await _distributedCache.RemoveCacheAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] REMOVE CACHE ERROR: {e.Message}");
            throw;
        }
    }

    public async Task SetStringAsync(string cacheKey, string value, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET STRING {cacheKey} TO {value} EXPIRATION {expire}");
            await _distributedCache.SetStringAsync(cacheKey, value, expire);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET STRING ERROR: {e.Message}");
            throw;
        }
    }

    public async Task<string> GetStringAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET STRING {cacheKey}");
            return await _distributedCache.GetStringAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET STRING ERROR: {e.Message}");
            throw;
        }
    }

    public async Task SetStringIncrementAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET STRING INCREMENT {cacheKey}");
            await _distributedCache.SetStringIncrementAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET STRING INCREMENT ERROR: {e.Message}");
            throw;
        }
    }

    public async Task SetExpireForKeyAsync(string cacheKey, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET EXPIRE FOR {cacheKey} TO {expire}");
            await _distributedCache.SetExpireForKeyAsync(cacheKey, expire);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET EXPIRE FOR KEY ERROR: {e.Message}");
            throw;
        }
    }

    public async Task<int> GetIncrementValueAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET INCREMENT VALUE {cacheKey}");
            return await _distributedCache.GetIncrementValueAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET INCREMENT VALUE ERROR: {e.Message}");
            throw;
        }
    }

    public async Task<TimeSpan?> GetKeyTimeToLiveAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET KEY TIME TO LIVE {cacheKey}");
            return await _distributedCache.GetKeyTimeToLiveAsync(cacheKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET KEY TIME TO LIVE ERROR: {e.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetKeyStringsAsync(string parentKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET KEY STRINGS WITH PARENT {parentKey}");
            return await _distributedCache.GetKeyStringsAsync(parentKey);
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET KEY STRINGS ERROR: {e.Message}");
            throw;
        }
    }
}
