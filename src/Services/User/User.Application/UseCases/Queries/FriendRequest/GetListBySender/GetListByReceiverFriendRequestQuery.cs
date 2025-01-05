using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using User.Application.DTOs.Response.FriendRequest;

namespace User.Application.UseCases.Queries.FriendRequest.GetListBySender;

public record GetListByReceiverFriendRequestQuery(PaginationRequest Query) : IQuery<PaginatedResult<FriendRequestByUserResDto>>;