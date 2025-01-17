using User.Application.Services;

namespace User.Application.UseCases.Commands.Block.CheckBlocked;

public class CheckBlockCommandHandler(IBlockService service) : ICommandHandler<CheckBlockCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CheckBlockCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CheckBlockAsync(request.UserId, cancellationToken);
        var response =  result
            ? Result.Success(result, "Check block successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest),"Failed to check block");

        return response; }
}