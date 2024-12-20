namespace Auth.Application.DTOs.Request.Auth;

public record ChangePasswordForAdminRequestDto(string UserId, string Password, string ConfirmPassword);