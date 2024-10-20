using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Repository.Cache;

public class DbCacheService
    : ICacheService
   
{
    public Task SetAsync(string cacheKey, object? value, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetAsync(string cacheKey)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string cacheKey)
    {
        throw new NotImplementedException();
    }
}