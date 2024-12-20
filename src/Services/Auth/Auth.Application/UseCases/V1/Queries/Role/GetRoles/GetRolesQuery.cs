using Auth.Application.DTOs.Response.Role;

namespace Auth.Application.UseCases.V1.Queries.Role.GetRoles;

public record GetRolesQuery : IQuery<IEnumerable<RoleResponseDto>>;