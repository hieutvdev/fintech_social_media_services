using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.FriendShip;
using User.Application.DTOs.Response.FriendShip;
using User.Application.Repositories;
using User.Application.Services;

namespace User.Infrastructure.Services;

public class FriendShipService
(IFriendShipRepository repository)
: IFriendShipService
{
    public async Task<bool> CreateAsync(CreateFriendShipReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteFriendShipReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        return await repository.GetFriendShipsByUserLoginAsync(paginationRequest, cancellationToken);
    }

    public async Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByAnotherUserLoginAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        return await repository.GetFriendShipsByAnotherUserLoginAsync(paginationRequest, cancellationToken);
    }

    public async Task<IEnumerable<FriendShipResBase>> GetFriendShipByUserIdsAsync(IEnumerable<string> userId, CancellationToken cancellationToken = default)
    {
        return await repository.GetFriendShipByUserIdsAsync(userId, cancellationToken);
    }
}