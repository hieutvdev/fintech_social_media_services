using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Role.UpdateRole;

public class UpdateRoleCommandHandler
(IRoleService roleService)
: ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await roleService.UpdateRoleAsync(request.UpdateRoleRequestDto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}