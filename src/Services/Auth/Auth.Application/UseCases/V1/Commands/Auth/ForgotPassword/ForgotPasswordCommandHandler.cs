using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler
(IAuthService authService)
: ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ForgotPasswordAsync(request.ForgotPasswordRequestDto, cancellationToken);
        var response = result ? Result.Success("Success") : Result.Failure(new Error("40001", "Forgot password wrong"));
        return response;
    }
}