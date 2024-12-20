namespace Auth.Application.DTOs.Request.Auth;

public record VerifyLoginCodeRequestDto(string Email, string Code);