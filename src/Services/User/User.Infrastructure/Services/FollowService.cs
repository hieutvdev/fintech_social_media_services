using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.Follow;
using User.Application.DTOs.Response.Follow;
using User.Application.Repositories;
using User.Application.Services;
using User.Application.UseCases.Models.Follow;

namespace User.Infrastructure.Services;

public class FollowService(IFollowRepository repository) : IFollowService
{
    public async Task<bool> CreateAsync(CreateFollowReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteFollowReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<bool> CheckFollowAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await repository.CheckFollowAsync(userId, cancellationToken);
    }

    public async Task<PaginatedResult<FollowResBaseDto>> GetListFollowAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListFollowAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<FollowResForUserDto>> GetListFollowerForUserLoginAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListFollowerForUserLoginAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<FollowResForUserDto>> GetListFollowingForUserLoginAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListFollowingForUserLoginAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<FollowResForOtherUser>> GetListFollowForOtherUserAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListFollowForOtherUserAsync(query, cancellationToken);
    }

    public async Task<bool> ConvertFollowToFriendAsync(ConvertFollowToFriendReqDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.ConvertFollowToFriendAsync(payload, cancellationToken);
    }
}