namespace Auth.Application.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string title) : base($"{title}")
    {
        
    }
}