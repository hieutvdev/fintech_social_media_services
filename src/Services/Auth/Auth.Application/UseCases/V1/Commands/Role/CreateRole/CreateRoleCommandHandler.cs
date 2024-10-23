using Auth.Application.Services;

namespace Auth.Application.UseCases.V1.Commands.Role.CreateRole;

public class CreateRoleCommandHandler(IRoleService roleService)
 : ICommandHandler<CreateRoleCommand, bool>
{
  public async Task<ResultT<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
  {
      var response = await roleService.CreateRoleAsync(request.CreateRoleRequestDto, cancellationToken);

      return Result.Create(response);
  }
}