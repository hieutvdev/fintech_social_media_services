using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Middleware;

public class IpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpLoggingMiddleware> _logger;


    public IpLoggingMiddleware(RequestDelegate next, ILogger<IpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();

        var request = context.Request;
        string path = request.Path;
        string requestMethod = request.Method;
        string url = path + request.QueryString;
        string clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "";
        var trackId = context.TraceIdentifier;
        string inputParam = "{}";
        string ipRequest = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() 
                           ?? context.Connection.RemoteIpAddress?.ToString();

        if (requestMethod == HttpMethod.Get.ToString())
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                var paramsFromClient = await reader.ReadToEndAsync();
                inputParam = paramsFromClient.Trim();
                request.Body.Position = 0;
            }
        }

        try
        {
            await _next(context);
            stopWatch.Stop();
            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                _logger.LogInformation(
                    "{Timespan} - INFO - {Url} - {TrackId} - IP:{IpAddress} -[[{clientIp}]]- [{Method}] - {ExecutionTime}ms - InputParams:{InputParams}",
                    DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    url,
                    trackId,
                    ipRequest,
                    clientIp,
                    requestMethod,
                    stopWatch.ElapsedMilliseconds,
                    inputParam);
            }
        }
        catch (Exception e)
        {
            await _next(context);
            stopWatch.Stop();
            _logger.LogError(
                "{Timespan} - ERROR - {Url} - {TrackId} - IP:{IpAddress} -[[{clientIp}]]- [{Method}] - {ExecutionTime}ms - Error:{error} - InputParams:{InputParams}",
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                url,
                trackId,
                ipRequest,
                clientIp,
                requestMethod,
                stopWatch.ElapsedMilliseconds,
                e.Message,
                inputParam);
        }
    }
}