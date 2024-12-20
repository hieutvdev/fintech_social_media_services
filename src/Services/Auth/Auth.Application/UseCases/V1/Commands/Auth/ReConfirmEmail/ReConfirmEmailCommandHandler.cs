using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.ReConfirmEmail;

public class ReConfirmEmailCommandHandler
(IAuthService authService)
: ICommandHandler<ReConfirmEmailCommand, bool>
{
    public async Task<ResultT<bool>> Handle(ReConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.ReConfirmEmailAsync(request.ReConfirmEmailRequestDto, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}