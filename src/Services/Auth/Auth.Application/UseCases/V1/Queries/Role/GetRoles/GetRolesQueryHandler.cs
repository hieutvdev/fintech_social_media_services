using Auth.Application.DTOs.Response.Role;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Role.GetRoles;

public class GetRolesQueryHandler
    (IRoleService roleService)
    : IQueryHandler<GetRolesQuery, IEnumerable<RoleResponseDto>>
{
    public async Task<ResultT<IEnumerable<RoleResponseDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var response = await roleService.GetRolesAsync(cancellationToken);
        return Result.Create(value: response);
    }
}