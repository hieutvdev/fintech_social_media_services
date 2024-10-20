using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;


namespace Auth.Application.UseCases.V1.Commands.Auth.RegisterUser;

public record RegisterUserCommand(RegisterRequestDto User) : ICommand<RegisterResponseDto>;