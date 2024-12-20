using Auth.Application.DTOs.Response.Role;

namespace Auth.Application.UseCases.V1.Queries.Role.GetRoleByUser;

public record GetRoleByUserQuery(string UserId) : IQuery<GetRoleByUserResponseDto>;