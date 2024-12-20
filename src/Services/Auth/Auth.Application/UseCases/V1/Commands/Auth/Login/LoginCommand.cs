
using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.UseCases.V1.Commands.Auth.Login;

public record LoginCommand(LoginRequestDto LoginRequestDto) : ICommand<LoginResponseDto>;