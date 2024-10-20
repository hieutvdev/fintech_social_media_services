namespace Auth.Application.DTOs.Response.Auth;

public record TokenResponse(string AccessToken, string RefreshToken);