using Auth.Application.DTOs.Request.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.ResetPassword;

public record ResetPasswordCommand(ResetPasswordRequestDto ResetPasswordRequestDto) : ICommand;