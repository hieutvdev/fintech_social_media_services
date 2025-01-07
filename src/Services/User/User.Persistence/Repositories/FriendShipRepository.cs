
using User.Application.DTOs.Request.FriendShip;
using User.Application.DTOs.Response.FriendShip;
using User.Application.Services;


namespace User.Persistence.Repositories;

public class FriendShipRepository
(IRepositoryBaseService<ApplicationDbContext> repository, IAuthorizeExtension authorizeExtension, IDistributedCacheService cacheService, IAuthServerService serverService) : IFriendShipRepository
{
    public async Task<bool> CreateAsync(CreateFriendShipReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var friendShipId = FriendShipId.Of(Guid.NewGuid());
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var friendShip = FriendShip.Create(friendShipId, userId, payload.FriendId);
            await repository.AddAsync(friendShip, cancellationToken);
            var isSuccessful = await repository.SaveChangesAsync(cancellationToken) > 0;
            if (isSuccessful)
            {
                await cacheService.RemoveCacheAsync(CacheKey.FriendShip.GetList);
            }
            return isSuccessful;
        }   
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteFriendShipReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var friendShipId = FriendShipId.Of(Guid.Parse(payload.Id));
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var isDeleted = await repository.ExecuteDeleteAsync<FriendShip>(r => r.Id == friendShipId && r.UserId == userId, cancellationToken);
            if (isDeleted)
            {
                await cacheService.RemoveCacheAsync(CacheKey.FriendShip.GetList);
            }
            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = authorizeExtension.GetUserFromClaimToken().Id;

            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByAnotherUserLoginAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}