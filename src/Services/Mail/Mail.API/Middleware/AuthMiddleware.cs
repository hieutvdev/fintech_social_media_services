using BuildingBlocks.Exceptions;
using Mail.API.Configurations;

namespace Mail.API.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.Request.Headers[RequestHeaderConfiguration.X_CLIENT_ID];
        if (string.IsNullOrEmpty(userId))
        {
            throw new BadRequestException("Invalid User");
        }

        await _next(context);
    }
    
}


