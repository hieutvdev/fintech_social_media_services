using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.ConfirmEmail;

public record ConfirmEmailCommand(ConfirmEmailRequestDto ConfirmEmailRequestDto) : ICommand<LoginResponseDto>;