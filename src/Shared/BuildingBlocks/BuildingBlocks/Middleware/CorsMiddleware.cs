using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Middleware;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorsMiddleware> _logger;


    public CorsMiddleware(RequestDelegate next, ILogger<CorsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" }); 
        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS" });
        context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type", "Authorization" });
        if (context.Request.Method == HttpMethods.Options)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return;
        }
        await _next(context);
    }
}