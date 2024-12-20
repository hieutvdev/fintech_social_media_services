namespace Upload.API.Exceptions;

public class FileUploadException : Exception
{
    public FileUploadException(string? message) : base(message)
    {
        
    }
    
    public FileUploadException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details;
}