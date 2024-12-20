namespace Auth.Application.UseCases.V1.Commands.Role.DeleteRole;

public record DeleteRoleCommand(string RoleName) : ICommand;