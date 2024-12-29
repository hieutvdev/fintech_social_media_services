using BuildingBlocks.Exceptions;

namespace User.Application.Exceptions;

public class UserTypeNotFoundException : NotFoundException
{
    public UserTypeNotFoundException(string message) : base(message)
    {

    }
}
    