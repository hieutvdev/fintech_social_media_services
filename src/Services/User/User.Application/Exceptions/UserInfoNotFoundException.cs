using BuildingBlocks.Exceptions;

namespace User.Application.Exceptions;

public class UserInfoNotFoundException : NotFoundException
{
    public UserInfoNotFoundException(string message) : base(message)
    {

    }
}