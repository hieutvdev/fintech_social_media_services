using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.Exceptions.ErrorCodeResponse;
using BuildingBlocks.Helpers;
using User.Application.Services;

namespace User.Application.UseCases.Commands.UserInfo.Create;

public class CreateUserInfoCommandHandler
(IUserInfoService service)
: ICommandHandler<CreateUserInfoCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateUserInfoAsync(request.Payload, cancellationToken);
        var response = result
            ? Result.Success(result, "Create user info successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest),"Failed to create user info");
        return response;
    }
}