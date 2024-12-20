namespace Auth.Application.DTOs.Response.Auth;

public record LoginResponseDto(
    UserDto UserData,
    TokenResponse TokenResponse
    );