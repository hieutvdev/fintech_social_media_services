using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.FriendRequest;
using User.Application.Services;

namespace User.Application.UseCases.Queries.FriendRequest.GetListByReceiver;

public class GetListBySenderFriendRequestQueryHandler(IFriendRequestService service) : IQueryHandler<GetListBySenderFriendRequestQuery, PaginatedResult<FriendRequestResBase>>
{
    public async Task<ResultT<PaginatedResult<FriendRequestResBase>>> Handle(GetListBySenderFriendRequestQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListBySenderAsync(request.Query, cancellationToken);
        var response = Result.Success(result, "Get list friend request by sender successfully");
        return response;
    }
}