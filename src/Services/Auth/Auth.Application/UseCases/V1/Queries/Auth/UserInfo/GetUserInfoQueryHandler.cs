using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Auth.UserInfo;

public class GetUserInfoQueryHandler(IAuthService service) : IQueryHandler<GetUserInfoQuery, UserDto>
{
    public async Task<ResultT<UserDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetUserByIdAsync(request.UserId, cancellationToken);
        var response = Result.Success(result, "Get user info successfully");
        return response;
    }
}