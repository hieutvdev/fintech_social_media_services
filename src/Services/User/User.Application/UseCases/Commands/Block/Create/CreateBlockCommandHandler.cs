using User.Application.Services;

namespace User.Application.UseCases.Commands.Block.Create;

public class CreateBlockCommandHandler
(IBlockService service)
: ICommandHandler<CreateBlockCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateBlockCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Payload, cancellationToken);
        var response =  result
            ? Result.Success(result, "Create block successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest),"Failed to create block");

        return response;
    }
}