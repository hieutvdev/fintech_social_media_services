using Auth.Application.DTOs.Request.Role;
using Auth.Application.DTOs.Response.Role;

namespace Auth.Application.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<RoleResponseDto>> GetRolesAsync(CancellationToken cancellationToken = default!);

    Task<bool> CreateRoleAsync(CreateRoleRequestDto createRoleRequestDto,
        CancellationToken cancellationToken = default!);

    Task<bool> DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default!);

    Task<bool> AssignRoleAsync(AssignRoleRequestDto assignRoleRequestDto,
        CancellationToken cancellationToken = default!);

    Task<bool> UpdateRoleAsync(UpdateRoleRequestDto updateRoleRequestDto, CancellationToken cancellationToken = default!);

    Task<UserByRoleResponseDto> GetUsersByRoleAsync(string roleName,
        CancellationToken cancellationToken = default!);


    Task<GetRoleByUserResponseDto> GetRoleByUserAsync(string userId, CancellationToken cancellationToken = default!);
}