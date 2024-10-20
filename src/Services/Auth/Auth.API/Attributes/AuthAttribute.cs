using Auth.Application.Services;
using Auth.Infrastructure.Configuration;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.API.Attributes;

public class AuthAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isAuthorized = context.ActionDescriptor.EndpointMetadata.Any(m => m is AuthorizeAttribute);
        if (isAuthorized)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtTokenService>();
        
            string accessTokenCacheKey = $"{CacheKey.Domain}{CacheKey.Auth.AccessToken}{jwtService.GetUserFromClaimToken().Id}{GetDeviceInfo(context.HttpContext)}";
            var cacheResponse = await cacheService.GetCacheAsync(accessTokenCacheKey);
            if (string.IsNullOrEmpty(cacheResponse))
            {
                throw new BadRequestException("Invalid token");
            }
        }
        

        await next();
    }
    
    
    
    private string GetDeviceInfo(HttpContext context)
    {
        var ipAddress = context!.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers.UserAgent.ToString();
        string deviceInfor = $"{ipAddress}-{userAgent}";

        while (deviceInfor.Contains(" ")) {
            deviceInfor = deviceInfor.Replace(" ", "");
        }
        return deviceInfor;
    }
}