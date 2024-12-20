using BuildingBlocks.Repository.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Logging;

public class LoggingDistributedCacheService : IDistributedCacheService
{
    
    private readonly ILoggingExtension<LoggingDistributedCacheService> _loggingExtension;

    private readonly IDistributedCacheService _distributedCache;   
    public LoggingDistributedCacheService(
        ILoggingExtension<LoggingDistributedCacheService> loggingExtension, IDistributedCacheService distributedCacheService)
    {
        _loggingExtension = loggingExtension;
        _distributedCache = distributedCacheService;
    
    }
    
    public Task SetCacheAsync(string cacheKey, object? value, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET CACHE VALUE {nameof(value)} TO {cacheKey} EXPIRATION {expire}");
        }
        catch(Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET CACHE ERROR: {e}");   
        }

        return Task.CompletedTask;
    }

    public Task<string> GetCacheAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET CACHE VALUE {cacheKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET CACHE ERROR: {e}"); 
        }

        return Task.FromResult("");
    }

    public Task RemoveCacheAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] REMOVE CACHE {cacheKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] REMOVE CACHE {e}");   
        }

        return Task.CompletedTask;
    }

    public Task SetStringAsync(string cacheKey, string value, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET STRING {cacheKey} TO {value} EXPIRATION {expire}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET STRING ERROR: {e}");
        }

        return Task.CompletedTask;
    }

    public Task<string> GetStringAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] GET STRING {cacheKey}");    
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] GET STRING ERROR: {e}");
        }

        return Task.FromResult("");

    }

    public Task SetStringIncrementAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"[REDIS] SET STRING INCREMENT {cacheKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"[REDIS] SET STRING INCREMENT {e}");
        }

        return Task.CompletedTask;
    }

    public Task SetExpireForKeyAsync(string cacheKey, TimeSpan expire)
    {
        try
        {
            _loggingExtension.Information($"");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"");
        }

        return Task.CompletedTask;
    }

    public Task<int> GetIncrementValueAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"GET INCREMENT VALUE {cacheKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"GET INCREMENT VALUE ERROR: {e}");
        }

        return Task.FromResult(0);
    }

    public Task<TimeSpan?> GetKeyTimeToLiveAsync(string cacheKey)
    {
        try
        {
            _loggingExtension.Information($"GET KEY TIME {cacheKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"GET KEY TIME {e}");
        }
        return Task.FromResult<TimeSpan?>(null);
    }

    public Task<IEnumerable<string>> GetKeyStringsAsync(string parentKey)
    {
        try
        {
            _loggingExtension.Information($"GET KEY STRINGS {parentKey}");
        }
        catch (Exception e)
        {
            _loggingExtension.Error($"GET KEY STRINGS {e}");
        }
        
        return Task.FromResult<IEnumerable<string>>([]);
    }
}