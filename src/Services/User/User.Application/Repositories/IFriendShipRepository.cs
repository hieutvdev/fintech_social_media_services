using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.FriendShip;
using User.Application.DTOs.Response.FriendShip;

namespace User.Application.Repositories;

public interface IFriendShipRepository
{
    Task<bool> CreateAsync(CreateFriendShipReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteAsync(DeleteFriendShipReqDto payload, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByAnotherUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);

    Task<IEnumerable<FriendShipResBase>> GetFriendShipByUserIdsAsync(IEnumerable<string> userId,
        CancellationToken cancellationToken = default!);

}