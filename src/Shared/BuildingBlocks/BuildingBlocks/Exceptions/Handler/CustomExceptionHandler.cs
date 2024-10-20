using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler
(ILogger<CustomExceptionHandler> logger)
: IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var stopWatch = Stopwatch.StartNew();

        var request = httpContext.Request;
        string path = request.Path;
        string requestMethod = request.Method;
        string url = path + request.QueryString;
        string ipRequest = httpContext.Connection.RemoteIpAddress?.ToString()!;
        var trackId = httpContext.TraceIdentifier;
        string inputParam = "{}";
        
        
        logger.LogError(
            "{Timespan} - ERROR - {Url} - {TrackId} - IP:{IpAddress} - [{Method}] - {ExecutionTime}ms - Error:{error} - InputParams:{InputParams}",
            DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            url,
            trackId,
            ipRequest,
            requestMethod,
            stopWatch.ElapsedMilliseconds,
            exception.Message,
            inputParam);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            UnauthorizedException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            AccessDeniedException => (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };
        
        
        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

}