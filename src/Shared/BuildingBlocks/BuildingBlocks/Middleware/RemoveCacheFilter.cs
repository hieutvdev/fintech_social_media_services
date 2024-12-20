using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ShredKernel.BaseClasses.Configurations;

namespace BuildingBlocks.Middleware;

public class RemoveCacheFilter : IEndpointFilter
{
    private readonly string _cacheKey;
    

    public RemoveCacheFilter(string cacheKey = "")
    {
        _cacheKey = cacheKey;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

        if (!cacheConfiguration.Enabled)
        {
            return await next(context);
        }
        
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IDistributedCacheService>();
        await cacheService.RemoveCacheAsync(this._cacheKey);
        return await next(context);
    }
}