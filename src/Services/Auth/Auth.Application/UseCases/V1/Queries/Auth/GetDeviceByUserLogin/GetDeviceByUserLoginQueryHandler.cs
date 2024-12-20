using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Auth.GetDeviceByUserLogin;

public class GetDeviceByUserLoginQueryHandler
(IAuthService authService)
: IQueryHandler<GetDeviceByUserLoginQuery, IEnumerable<DeviceLoginResponseDto>>
{
    public async Task<ResultT<IEnumerable<DeviceLoginResponseDto>>> Handle(GetDeviceByUserLoginQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentNullException("Value cannot be empty or whitespace");
        }
        var result = await authService.GetDeviceLoginByUserAsync(request.Email, cancellationToken);
        var response = Result.Create(result);

        return response;
    }
}