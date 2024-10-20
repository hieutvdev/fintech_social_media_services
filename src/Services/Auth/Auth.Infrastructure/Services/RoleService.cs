using Auth.Application.DTOs.Request.Role;
using Auth.Application.DTOs.Response.Role;
using Auth.Application.Repositories;
using Auth.Application.Services;

namespace Auth.Infrastructure.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<IEnumerable<RoleResponseDto>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetRolesAsync(cancellationToken);
    }

    public async Task<bool> CreateRoleAsync(CreateRoleRequestDto createRoleRequestDto, CancellationToken cancellationToken = default)
    {
        return await roleRepository.CreateRoleAsync(createRoleRequestDto, cancellationToken);
    }

    public async Task<bool> DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await roleRepository.DeleteRoleAsync(roleName, cancellationToken);
    }

    public async Task<bool> AssignRoleAsync(AssignRoleRequestDto assignRoleRequestDto, CancellationToken cancellationToken = default)
    {
        return await roleRepository.AssignRoleAsync(assignRoleRequestDto, cancellationToken);
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleRequestDto updateRoleRequestDto, CancellationToken cancellationToken = default)
    {
        return await roleRepository.UpdateRoleAsync(updateRoleRequestDto, cancellationToken);
    }

    public async Task<UserByRoleResponseDto> GetUsersByRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetUsersByRoleAsync(roleName, cancellationToken);
    }

    public async Task<GetRoleByUserResponseDto> GetRoleByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await roleRepository.GetRoleByUserAsync(userId, cancellationToken);
    }
}