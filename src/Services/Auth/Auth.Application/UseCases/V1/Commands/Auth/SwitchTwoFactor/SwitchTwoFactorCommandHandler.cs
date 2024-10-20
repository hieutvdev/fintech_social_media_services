using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.SwitchTwoFactor;

public class SwitchTwoFactorCommandHandler
(IAuthService authService) 
: ICommandHandler<SwitchTwoFactorCommand>
{
    public async Task<Result> Handle(SwitchTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.SwitchTwoFactorAsync(cancellationToken);
        var response = result ? Result.Success() : Result.Failure(new Error("40001", "Switch two factor failure"));
        return response;
    }
}