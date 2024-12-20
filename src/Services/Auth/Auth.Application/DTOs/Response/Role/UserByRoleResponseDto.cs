using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.DTOs.Response.Role;

public record UserByRoleResponseDto(RoleResponseDto ResponseDto, IEnumerable<UserDto> UserDtos);
