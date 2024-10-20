namespace Auth.Application.DTOs.Response.Role;

public record RoleResponseDto(string Id, string? RoleName, string Description, DateTime? CreateAt);