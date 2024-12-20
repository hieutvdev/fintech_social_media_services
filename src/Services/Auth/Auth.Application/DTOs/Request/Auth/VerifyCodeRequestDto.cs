namespace Auth.Application.DTOs.Request.Auth;

public record VerifyCodeRequestDto(string Email, string Code);