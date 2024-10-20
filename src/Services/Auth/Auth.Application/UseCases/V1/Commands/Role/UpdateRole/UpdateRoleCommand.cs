using Auth.Application.DTOs.Request.Role;

namespace Auth.Application.UseCases.V1.Commands.Role.UpdateRole;

public record UpdateRoleCommand(UpdateRoleRequestDto UpdateRoleRequestDto) : ICommand;