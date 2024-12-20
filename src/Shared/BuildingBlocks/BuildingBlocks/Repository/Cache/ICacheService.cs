namespace BuildingBlocks.Repository.Cache;

public interface ICacheService
{
    Task SetAsync(string cacheKey, object? value, TimeSpan? expiration = null);
    Task<string> GetAsync(string cacheKey);
    Task RemoveAsync(string cacheKey);

}