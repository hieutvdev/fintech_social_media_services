using User.Application.Services;

namespace User.Application.UseCases.Commands.Block.Delete;

public class DeleteBlockCommandHandler
(IBlockService service) : ICommandHandler<DeleteBlockCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteBlockCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Payload, cancellationToken);
        var response =  result
            ? Result.Success(result, "Delete block successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest),"Failed to delete block");

        return response;
    }
}