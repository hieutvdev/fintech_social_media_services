namespace Auth.Application.DTOs.Request.Auth;

public record ResetPasswordRequestDto(string Email, string Token, string NewPassword, string ConfirmPassword);