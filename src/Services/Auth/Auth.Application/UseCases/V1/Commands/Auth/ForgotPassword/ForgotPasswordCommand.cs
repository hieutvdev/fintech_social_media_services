using Auth.Application.DTOs.Request.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordRequestDto ForgotPasswordRequestDto) : ICommand;