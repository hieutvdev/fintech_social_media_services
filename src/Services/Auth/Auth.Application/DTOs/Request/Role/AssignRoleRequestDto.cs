namespace Auth.Application.DTOs.Request.Role;

public record AssignRoleRequestDto(string UserName, IEnumerable<string> RoleNames);