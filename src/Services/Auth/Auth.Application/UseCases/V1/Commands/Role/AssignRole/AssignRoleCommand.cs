using Auth.Application.DTOs.Request.Role;

namespace Auth.Application.UseCases.V1.Commands.Role.AssignRole;

public record AssignRoleCommand(AssignRoleRequestDto AssignRoleRequestDto) : ICommand;