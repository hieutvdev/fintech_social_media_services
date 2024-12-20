namespace Auth.Application.DTOs.Response.Auth;

public record RegisterResponseDto(UserDto UserData, TokenResponse Token);