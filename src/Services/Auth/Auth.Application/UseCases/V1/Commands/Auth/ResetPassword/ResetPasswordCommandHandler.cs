using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler
(IAuthService authService)
: ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ResetPasswordAsync(request.ResetPasswordRequestDto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}