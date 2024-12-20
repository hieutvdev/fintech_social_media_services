using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BuildingBlocks.Behaviors;

public class PerformancePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _stopwatch;
    private readonly ILogger<TRequest> _logger;

    public PerformancePipelineBehavior(ILogger<TRequest> logger)
    {
        _stopwatch = new Stopwatch();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _stopwatch.Start();

        var response = await next();

        _stopwatch.Stop();

        if (_stopwatch.ElapsedMilliseconds <= 5000) return response;

        var name = typeof(TRequest).Name;
        _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, _stopwatch.ElapsedMilliseconds, request);
        return response;
    }
}