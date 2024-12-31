using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.Exceptions.ErrorCodeResponse;
using BuildingBlocks.Helpers;
using User.Application.Services;

namespace User.Application.UseCases.Commands.UserInfo.Update;

public class UpdateUserInfoCommandHandler
(IUserInfoService service) 
: ICommandHandler<UpdateUserInfoCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateUserInfoAsync(request.Payload, cancellationToken);
        var response = result
            ? Result.Success(result, "Update user info successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest), "Failed to update user info");
        return response;
    }
}