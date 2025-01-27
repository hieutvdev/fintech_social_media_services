using BuildingBlocks.Pagination;
using User.Application.DTOs.Request.Follow;
using User.Application.DTOs.Response.Follow;
using User.Application.UseCases.Models.Follow;

namespace User.Application.Repositories;

public interface IFollowRepository
{
    Task<bool> CreateAsync(CreateFollowReqDto payload, CancellationToken cancellationToken = default!);
    Task<bool> DeleteAsync(DeleteFollowReqDto payload, CancellationToken cancellationToken = default!);
    Task<bool> CheckFollowAsync(string userId, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<FollowResBaseDto>> GetListFollowAsync(FollowSearchListModelQuery query,
        CancellationToken cancellationToken = default!);
    Task<PaginatedResult<FollowResForUserDto>> GetListFollowerForUserLoginAsync(FollowSearchListModelQuery query,
        CancellationToken cancellationToken = default!);
    Task<PaginatedResult<FollowResForUserDto>> GetListFollowingForUserLoginAsync(FollowSearchListModelQuery query,
        CancellationToken cancellationToken = default!);
    Task<PaginatedResult<FollowResForOtherUser>> GetListFollowForOtherUserAsync(FollowSearchListModelQuery query,
        CancellationToken cancellationToken = default!);
    Task<bool> ConvertFollowToFriendAsync(ConvertFollowToFriendReqDto payload,
        CancellationToken cancellationToken = default!);

}