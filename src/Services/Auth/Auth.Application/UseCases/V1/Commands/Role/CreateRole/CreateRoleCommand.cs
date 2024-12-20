using Auth.Application.DTOs.Request.Role;

namespace Auth.Application.UseCases.V1.Commands.Role.CreateRole;

public record CreateRoleCommand(CreateRoleRequestDto CreateRoleRequestDto) : ICommand<bool>;