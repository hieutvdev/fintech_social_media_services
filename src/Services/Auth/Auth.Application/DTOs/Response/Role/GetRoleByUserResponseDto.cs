using Auth.Application.DTOs.Response.Auth;

namespace Auth.Application.DTOs.Response.Role;

public record GetRoleByUserResponseDto(UserDto UserDto, IEnumerable<RoleResponseDto> ResponseDtos);