using Auth.Application.DTOs.Response.Auth;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Auth.GetUserShare;

public class GetUserShareQueryHandler(IAuthService service) : IQueryHandler<GetUserShareQuery, IEnumerable<UserShareResponseDto>>
{
    public async Task<ResultT<IEnumerable<UserShareResponseDto>>> Handle(GetUserShareQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetUserShareAsync(request.Query, cancellationToken);
        var response = Result.Success(result, "Get user share successfully");
        return response;
    }
}