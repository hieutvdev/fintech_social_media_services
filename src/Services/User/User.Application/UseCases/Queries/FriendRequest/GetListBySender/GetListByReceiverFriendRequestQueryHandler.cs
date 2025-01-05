using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.FriendRequest;
using User.Application.Services;

namespace User.Application.UseCases.Queries.FriendRequest.GetListBySender;

public class GetListByReceiverFriendRequestQueryHandler
(IFriendRequestService service) : IQueryHandler<GetListByReceiverFriendRequestQuery, PaginatedResult<FriendRequestByUserResDto>>
{
    public async Task<ResultT<PaginatedResult<FriendRequestByUserResDto>>> Handle(GetListByReceiverFriendRequestQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListByReceiverIdAsync(request.Query, cancellationToken);
        var response = Result.Success(result, "Get list friend request by receiver successfully");
        return response;
    }
}