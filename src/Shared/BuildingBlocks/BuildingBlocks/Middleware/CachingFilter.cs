using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ShredKernel.BaseClasses.Configurations;

namespace BuildingBlocks.Middleware;

public class CachingFilter : IEndpointFilter
{
    private readonly int _timeToLiveSeconds;

    public CachingFilter(int timeToLiveSeconds = 1000)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

        if (!cacheConfiguration.Enabled)
        {
            return await next(context); 
        }

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IDistributedCacheService>();
        var cacheKey = CachingHelper.GenerateCacheKeyFromRequest(context.HttpContext.Request);

  
        var cacheResponse = await cacheService.GetCacheAsync(cacheKey);
        if (!string.IsNullOrEmpty(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            
            return contentResult;
        }


        var executedContext = await next(context);


        if (executedContext is OkObjectResult objectResult)
        {
            var serializedResponse = objectResult.Value is not null 
                ? System.Text.Json.JsonSerializer.Serialize(objectResult.Value) 
                : string.Empty;
                
            await cacheService.SetCacheAsync(cacheKey, serializedResponse, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }

        return executedContext;
    }

   
}
