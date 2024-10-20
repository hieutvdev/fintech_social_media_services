

using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.Login;

internal class LoginCommandHandler(IAuthService authService) : ICommandHandler<LoginCommand, LoginResponseDto>
{
    public async Task<ResultT<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request.LoginRequestDto, cancellationToken);

        var response = Result.Create(value: result);

        return response;
    }
}
