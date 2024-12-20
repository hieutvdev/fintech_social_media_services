using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;


namespace Auth.Application.UseCases.V1.Commands.Auth.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterResponseDto>
{
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ResultT<RegisterResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request.User, cancellationToken);
        
        var response = Result.Create(value: result);
        return response;
    }
}