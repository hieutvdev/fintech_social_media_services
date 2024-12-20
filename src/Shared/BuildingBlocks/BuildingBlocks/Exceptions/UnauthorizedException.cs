namespace BuildingBlocks.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
        
    }

    public UnauthorizedException() : this("You are not authorized to access this resource.")
    {
        
    }

    public UnauthorizedException(string message, Exception innerException, string details) : base(message,
        innerException)
    {
        Details = details;
    }

    public string? Details;
}