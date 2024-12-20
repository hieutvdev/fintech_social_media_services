using Auth.Application.DTOs.Response.Role;
using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Queries.Role.GetUserByRole;

public class GetUserByRoleQueryHandler
(IRoleService roleService)
: IQueryHandler<GetUserByRoleQuery, UserByRoleResponseDto>
{
    public async Task<ResultT<UserByRoleResponseDto>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RoleName))
        {
            throw new BadRequestException("Role name cannot be empty or white space");
        }
        var result = await roleService.GetUsersByRoleAsync(request.RoleName, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}