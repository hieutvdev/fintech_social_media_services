using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

using User.Application.Services;

namespace User.Application.UseCases.Commands.FriendRequest.Delete;

public class DeleteFriendRequestCommandHandler(IFriendRequestService service) : ICommandHandler<DeleteFriendRequestCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Payload, cancellationToken);
        var response = result ? Result.Success(result, "Delete friend request successfully") : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest), "Failed to delete friend request");
        return response;
    }
}