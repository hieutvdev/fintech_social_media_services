using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CacheServiceFactory _cacheServiceFactory;
    private readonly ILogger<AuthMiddleware> _logger;
    

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        await _next(context);
    }
    
    
}