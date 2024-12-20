using Auth.Application.DTOs.Response.Role;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Role.GetRoleByUser;

public class GetRoleByUserQueryHandler
(IRoleService roleService)
: IQueryHandler<GetRoleByUserQuery, GetRoleByUserResponseDto>
{
    public async Task<ResultT<GetRoleByUserResponseDto>> Handle(GetRoleByUserQuery request, CancellationToken cancellationToken)
    {
        var result = await roleService.GetRoleByUserAsync(request.UserId, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}