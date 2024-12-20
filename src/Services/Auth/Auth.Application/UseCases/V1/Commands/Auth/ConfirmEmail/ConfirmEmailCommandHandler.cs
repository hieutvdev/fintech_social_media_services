using Auth.Application.DTOs.Request.Auth;
using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.ConfirmEmail;

public class ConfirmEmailCommandHandler
(IAuthService authService)
: ICommandHandler<ConfirmEmailCommand, LoginResponseDto>
{
    public async Task<ResultT<LoginResponseDto>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ConfirmEmailAsync(request.ConfirmEmailRequestDto, cancellationToken);
        var response =  Result.Create(value: result);
        return response;
    }
}