using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.FriendRequest;

namespace User.Application.UseCases.Queries.FriendRequest.GetListByReceiver;

public record GetListBySenderFriendRequestQuery(PaginationRequest Query) : IQuery<PaginatedResult<FriendRequestByUserResDto>>;