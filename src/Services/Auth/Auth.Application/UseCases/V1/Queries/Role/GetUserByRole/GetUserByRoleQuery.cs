using Auth.Application.DTOs.Response.Role;

namespace Auth.Application.UseCases.V1.Queries.Role.GetUserByRole;

public record GetUserByRoleQuery(string RoleName) : IQuery<UserByRoleResponseDto>;