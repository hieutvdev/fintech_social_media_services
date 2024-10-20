namespace Auth.Application.DTOs.Response.Auth;

public record UserDto(string Id, string UserName, string FullName, string AvatarUrl, string BirthDay);
