namespace BuildingBlocks.Logging;

public interface ILoggingExtension<T> where T : class
{
    void Information(string message);
    void Warning(string message);
    void Error(string message);
    void Error(Exception exception, string message = null);
    void Debug(string message);
    void Trace(string message);
    
}