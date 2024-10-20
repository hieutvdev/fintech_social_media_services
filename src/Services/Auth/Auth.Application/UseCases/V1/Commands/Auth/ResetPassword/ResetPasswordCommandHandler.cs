using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler
(IAuthService authService)
: ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ResetPasswordAsync(request.ResetPasswordRequestDto, cancellationToken);
        var response = result ? Result.Success() : Result.Failure(new Error("40001", "Reset password failure"));
        return response;
    }
}