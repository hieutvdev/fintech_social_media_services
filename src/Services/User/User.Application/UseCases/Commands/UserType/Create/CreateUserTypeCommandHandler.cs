using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using User.Application.Services;

namespace User.Application.UseCases.Commands.UserType.Create;

public class CreateUserTypeCommandHandler
(IUserTypeService service)
: ICommandHandler<CreateUserTypeCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateUserTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Payload, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}