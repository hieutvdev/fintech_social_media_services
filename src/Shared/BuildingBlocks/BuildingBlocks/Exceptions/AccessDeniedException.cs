namespace BuildingBlocks.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) : base(message)
    {
        
    }

    public AccessDeniedException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details;
}