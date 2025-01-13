using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthMiddleware> _logger;
    

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        var authorizeMetadata = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>();
        if (authorizeMetadata is not null)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                _logger.LogInformation("Missing Authorization Header");
                throw new UnauthorizedException("Missing Authorization Header");
            }
        }
        
        await _next(context);
    }
    
    
}