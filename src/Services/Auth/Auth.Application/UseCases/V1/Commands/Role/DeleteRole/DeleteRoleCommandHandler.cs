using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Role.DeleteRole;

public class DeleteRoleCommandHandler
(IRoleService roleService)
: ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.DeleteRoleAsync(request.RoleName, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}