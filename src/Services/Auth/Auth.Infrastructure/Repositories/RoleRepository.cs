using Auth.Application.DTOs.Request.Role;
using Auth.Application.DTOs.Response.Auth;
using Auth.Application.DTOs.Response.Role;
using Auth.Application.Exceptions;
using Auth.Application.Repositories;
using Auth.Application.Services;
using Auth.Domain.Models;
using AutoMapper;
using BuildingBlocks.Exceptions;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Auth.Infrastructure.Repositories;

public class RoleRepository(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService, IMapper mapper)
: IRoleRepository
{
    
    public async Task<IEnumerable<RoleResponseDto>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<RoleResponseDto> roles =
            await roleManager.Roles.AsNoTracking().Select(r => new RoleResponseDto(r.Id, r.Name, r.Descriptions, r.CreatedAt)).ToListAsync<RoleResponseDto>(cancellationToken);

        return roles;
    }

    public async Task<bool> CreateRoleAsync(CreateRoleRequestDto createRoleRequestDto, CancellationToken cancellationToken = default)
    {
        
        var roleFound = await roleManager.FindByNameAsync(createRoleRequestDto.RoleName);
        if (roleFound is not null)
        {
            return false;
        }

        IdentityResult result =  await roleManager.CreateAsync(new ApplicationRole
        {
            Descriptions = createRoleRequestDto.Description,
            Name = createRoleRequestDto.RoleName

        });

        return result.Succeeded;

    }

    public async Task<bool> DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var roleFound = await roleManager.FindByNameAsync(roleName);
            if (roleFound is null)
            {
                return false;
            }

            IdentityResult isDeleteRole = await roleManager.DeleteAsync(roleFound);
            return isDeleteRole.Succeeded;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> AssignRoleAsync(AssignRoleRequestDto assignRoleRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByNameAsync(assignRoleRequestDto.UserName);
            if (user is null)
            {
                throw new UserNotFoundException(
                    $"Could not find user with username is {assignRoleRequestDto.UserName}");
            }
            foreach (var roleName in assignRoleRequestDto.RoleNames)
            {
                var roleExits = await roleManager.FindByNameAsync(roleName);
                if (roleExits is null)
                {
                    throw new RoleNotFoundException($"Could not find role with name {roleName}");
                }

                var userAssignRoleExits = await userManager.GetRolesAsync(user);
                IdentityResult isAddRoleSuccess = userAssignRoleExits.Any(r => r == roleName) ? throw new BadRequestException("") : await userManager.AddToRoleAsync(user, roleName);
                if (!isAddRoleSuccess.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateRoleAsync(UpdateRoleRequestDto updateRoleRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var roleFound = await roleManager.FindByNameAsync(updateRoleRequestDto.RoleName);
            if (roleFound is null)
            {
                return true;
            }

            roleFound.Name = updateRoleRequestDto.RoleName;
            roleFound.NormalizedName = updateRoleRequestDto.RoleName;
            roleFound.Descriptions = updateRoleRequestDto.Description;
            IdentityResult isUpdated = await roleManager.UpdateAsync(roleFound);
            return isUpdated.Succeeded;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<UserByRoleResponseDto> GetUsersByRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var roleFound = await roleManager.FindByNameAsync(roleName);
            if (roleFound is null)
            {
                throw new RoleNotFoundException($"Cloud not find role with name {roleName}");
            }

            var userInRoles = await userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<UserDto> userDtos = mapper.Map<IEnumerable<UserDto>>(userInRoles);
            RoleResponseDto roleResponseDto = mapper.Map<RoleResponseDto>(roleFound);
            return new UserByRoleResponseDto(roleResponseDto, userDtos);

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<GetRoleByUserResponseDto> GetRoleByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userFound = await userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException("Cannot be to find user");
            var roleByUsers = await userManager.GetRolesAsync(userFound);
            var userDto = mapper.Map<UserDto>(userFound);
            var roleDtos = mapper.Map<IEnumerable<RoleResponseDto>>(roleByUsers);
            var response = new GetRoleByUserResponseDto(userDto, roleDtos);
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}