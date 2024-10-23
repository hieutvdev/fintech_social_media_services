using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Role.AssignRole;

public class AssignRoleCommandHandler
(IRoleService roleService)
: ICommandHandler<AssignRoleCommand>
{
    public async Task<Result> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.AssignRoleAsync(request.AssignRoleRequestDto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}