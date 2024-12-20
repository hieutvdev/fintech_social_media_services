namespace Auth.Application.DTOs.Request.Auth;

public record LoginRequestDto(string UserName, string Password, string Code = "");