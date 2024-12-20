using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ServiceProvider _serviceProvider;

    public RateLimitMiddleware(RequestDelegate next, ServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string clientIp = context.Connection.RemoteIpAddress?.ToString()!;
        string key = $"RateLimit-{clientIp}";
        using (var scope = _serviceProvider.CreateScope())
        {
            var redisService = scope.ServiceProvider.GetRequiredService<RedisCacheService>();

            var currentCount = await redisService.GetAsync(key);
        }
    }
}