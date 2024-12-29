using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using User.Application.Services;

namespace User.Application.UseCases.Commands.UserType.Delete;

public class DeleteUserTypeCommandHandler
(IUserTypeService service)
: ICommandHandler<DeleteUserTypeCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteUserTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Payload, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}