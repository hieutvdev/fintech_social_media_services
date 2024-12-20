namespace Auth.Application.DTOs.Request.Auth;

public record ConfirmEmailRequestDto(string Token, string Email);