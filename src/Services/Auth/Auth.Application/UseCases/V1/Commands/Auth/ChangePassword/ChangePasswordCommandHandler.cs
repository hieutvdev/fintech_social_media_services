
using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;
using MediatR;

namespace Auth.Application.UseCases.V1.Commands.Auth.ChangePassword;

public class ChangePasswordCommandHandler(IAuthService authService)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ChangePasswordAsync(request.ChangePasswordRequestDto, cancellationToken);
        if (!result)
        {
            return Result.Failure(new Error("400", "ChangePassword failure"));
        }

        var response = Result.Create(result);
        return response;
    }
}
