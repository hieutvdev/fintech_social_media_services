using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Auth.LogoutDevice;

public class LogoutDeviceCommandHandler
(IAuthService authService)
: ICommandHandler<LogoutDeviceCommand>
{
    public async Task<Result> Handle(LogoutDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.LogoutDeviceAsync(request.DeviceLoginResponseDtos, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}