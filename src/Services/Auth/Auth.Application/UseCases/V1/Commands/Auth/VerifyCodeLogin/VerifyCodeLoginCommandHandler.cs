using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.VerifyCodeLogin;

public class VerifyCodeLoginCommandHandler
(IAuthService authService)
: ICommandHandler<VerifyCodeLoginCommand, LoginResponseDto>
{
    public async Task<ResultT<LoginResponseDto>> Handle(VerifyCodeLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.VerifyLoginCodeAsync(request.verifyLoginCodeRequestDto, cancellationToken);
        var response =
            Result.Create(value: result);
           
        return response;
    }
}