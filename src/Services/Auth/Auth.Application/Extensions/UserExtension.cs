using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.Extensions;

public static class UserExtension
{


    public static UserDto ApplicationUserToUserDto(ApplicationUser user)
        => new UserDto(user.Id, user.UserName!, user.FullName, user.AvatarUrl, user.BirthDay);

}