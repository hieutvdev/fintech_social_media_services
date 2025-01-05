
using User.Application.Services;

namespace User.Application.UseCases.Commands.FriendRequest.Create;

public class CreateFriendRequestCommandHandler
(IFriendRequestService service)
: ICommandHandler<CreateFriendRequestCommand, bool>

{
    public async Task<ResultT<bool>> Handle(CreateFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Payload, cancellationToken);
        var response = result
            ? Result.Success(result, "Create friend request successfully")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest),"Failed to create friend request");

        return response;
    }
}