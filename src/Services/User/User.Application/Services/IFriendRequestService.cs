using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.FriendRequest;
using User.Application.DTOs.Response.FriendRequest;

namespace User.Application.Services;

public interface IFriendRequestService
{
    Task<bool> CreateAsync(CreateFriendRequestReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> UpdateAsync(UpdateStatusFriendRequestReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteAsync(DeleteFriendRequestReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<FriendRequestByUserResDto>> GetListBySenderAsync(PaginationRequest query, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<FriendRequestByUserResDto>> GetListByReceiverIdAsync(PaginationRequest query, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<FriendRequestByUserLoginResDto>> GetListByUserLoginAsync(PaginationRequest query, CancellationToken cancellationToken = default!);
}