namespace Auth.Application.DTOs.Request.Auth;

public record RegisterRequestDto(string UserName, string Email, string Password, string FullName, string AvatarUrl, string BirthDay);
