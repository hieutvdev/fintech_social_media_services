using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using User.Application.Services;

namespace User.Application.UseCases.Commands.UserType.Update;

public class UpdateUserTypeCommandHandler
(IUserTypeService service)
: ICommandHandler<UpdateUserTypeCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateUserTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request.Payload, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}