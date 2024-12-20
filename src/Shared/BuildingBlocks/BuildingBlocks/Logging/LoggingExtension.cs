using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Logging;

public class LoggingExtension<T> : ILoggingExtension<T> where T : class 
{
    private readonly ILogger<T> _logger;

    public LoggingExtension(ILogger<T> logger)
    {
        _logger = logger;
    }

    private string FormatMessage(string message)
    {
        return $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] --- {message}";
    }
    
    public void Information(string message)
    {
        _logger.LogInformation(FormatMessage(message));
    }

    public void Warning(string message)
    {
        _logger.LogWarning(FormatMessage(message));
    }

    public void Error(string message)
    {
        _logger.LogError(FormatMessage(message));
    }

    public void Error(Exception exception, string message = null)
    {
        _logger.LogError(exception, FormatMessage(message));
    }

    public void Debug(string message)
    {
        _logger.LogDebug(FormatMessage(message));
    }

    public void Trace(string message)
    {
        _logger.LogTrace(FormatMessage(message));
    }
}