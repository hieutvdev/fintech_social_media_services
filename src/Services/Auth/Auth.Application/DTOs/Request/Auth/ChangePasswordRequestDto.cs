namespace Auth.Application.DTOs.Request.Auth;

public record ChangePasswordRequestDto(string OldPassword, string NewPassword, string ConfirmPassword);