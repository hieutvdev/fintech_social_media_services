using Auth.Application.Services;
using Auth.Infrastructure.Configuration;
using BuildingBlocks.Exceptions;

namespace Auth.API.Middlewares;

public class AuthMiddlewareV1
{
    private readonly RequestDelegate _next;

    public AuthMiddlewareV1(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        
        var authorizeMetadata = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>();

        if (authorizeMetadata != null)
        {
            var cacheService = context.RequestServices.GetRequiredService<ICacheService>();
            var jwtService = context.RequestServices.GetRequiredService<IJwtTokenService>();
            
            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{jwtService.GetUserFromClaimToken().Id}{GetDeviceInfo(context)}";
            var cacheResponse = await cacheService.GetCacheAsync(accessTokenCacheKey);

            if (string.IsNullOrEmpty(cacheResponse))
            {
                throw new BadRequestException("Invalid token");
                return;
            }
        }

        await _next(context);
    }

    private string GetDeviceInfo(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        string deviceInfo = $"{ipAddress}-{userAgent}";

        return deviceInfo.Replace(" ", "");
    }
}