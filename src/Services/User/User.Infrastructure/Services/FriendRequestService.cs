using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.FriendRequest;
using User.Application.DTOs.Response.FriendRequest;
using User.Application.Repositories;
using User.Application.Services;

namespace User.Infrastructure.Services;

public class FriendRequestService
(IFriendRequestRepository repository) : IFriendRequestService
{
    public async Task<bool> CreateAsync(CreateFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateStatusFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.UpdateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteFriendRequestReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<PaginatedResult<FriendRequestByUserResDto>> GetListBySenderAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListBySenderAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<FriendRequestByUserResDto>> GetListByReceiverIdAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListByReceiverIdAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<FriendRequestByUserLoginResDto>> GetListByUserLoginAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListByUserLoginAsync(query, cancellationToken);
    }
}