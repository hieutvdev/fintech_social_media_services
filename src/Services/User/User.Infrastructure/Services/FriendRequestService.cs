using BuildingBlocks.Pagination;

using Serilog;
using User.Application.DTOs.Request.FriendRequest;
using User.Application.DTOs.Response.FriendRequest;
using User.Application.Repositories;
using User.Application.Services;

namespace User.Infrastructure.Services;

public class FriendRequestService
(IFriendRequestRepository repository, IAuthServerService authServerService) : IFriendRequestService
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

    public async Task<PaginatedResult<FriendRequestResBase>> GetListBySenderAsync(PaginationRequest query, CancellationToken cancellationToken = default)
    {
        var friendRequest = await repository.GetListBySenderAsync(query, cancellationToken);
        var userInfo = await authServerService.GetUserShareAsync(friendRequest.DataResponse.Select(r => r.ReceiverId).ToList(), cancellationToken);
        var result = friendRequest.DataResponse.Select(r => new FriendRequestResBase(r.Id, r.ReceiverId, r.SendAt, userInfo.FirstOrDefault(u => u.Id == r.ReceiverId)?.FullName, userInfo.FirstOrDefault(u => u.Id == r.ReceiverId)?.Avatar));
        var response = new PaginatedResult<FriendRequestResBase>(friendRequest.PageIndex, friendRequest.PageSize,
            friendRequest.TotalRecord, result);
        return response;
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