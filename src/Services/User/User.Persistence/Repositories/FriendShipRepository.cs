
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
            var cacheKey = $"{string.Format(CacheKey.FriendShip.GetList, userId)}{CachingHelper.ObjectToQueryString(paginationRequest)}";
            var cacheValue = await cacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                var resCache = JsonConvert.DeserializeObject<PaginatedResult<FriendShipResBase>>(cacheValue);
                return resCache ?? new PaginatedResult<FriendShipResBase>(paginationRequest.PageIndex, paginationRequest.PageSize, 0, new List<FriendShipResBase>());
            }
            var friendShips = from i in  repository.Table<FriendShip>()
                .Select(f => new
                {
                    Id = f.Id,
                    FriendId = f.FriendId,
                    UserId = f.UserId
                })
                .Where(f => f.UserId == userId)

                join ii in repository.Table<UserInfo>().Select(r => new
                {
                    UserId = r.UserId,
                    FullName = r.FullName,
                    AvatarUrl = r.AvatarUrl
                }) on i.UserId equals ii.UserId
                    select new FriendShipResBase(i.Id.Value.ToString(),i.FriendId, ii.FullName, ii.AvatarUrl);
            var response = new PaginatedResult<FriendShipResBase>(paginationRequest.PageIndex, paginationRequest.PageSize, friendShips.Count(), friendShips.ToList());
            await cacheService.SetCacheAsync(cacheKey, response, TimeSpan.FromMinutes(10));
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FriendShipResBase>> GetFriendShipsByAnotherUserLoginAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var cacheKey = $"{string.Format(CacheKey.FriendShip.GetList, userId)}{CachingHelper.ObjectToQueryString(paginationRequest)}";
            var cacheValue = await cacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                var resCache = JsonConvert.DeserializeObject<PaginatedResult<FriendShipResBase>>(cacheValue);
                return resCache ?? new PaginatedResult<FriendShipResBase>(paginationRequest.PageIndex, paginationRequest.PageSize, 0, new List<FriendShipResBase>());
            }
            var friendShips = from i in  repository.Table<FriendShip>()
                .Select(f => new
                {
                    Id = f.Id,
                    FriendId = f.FriendId,
                    UserId = f.UserId
                })
                .Where(f => f.FriendId == userId)

                join ii in repository.Table<UserInfo>().Select(r => new
                {
                    UserId = r.UserId,
                    FullName = r.FullName,
                    AvatarUrl = r.AvatarUrl
                }) on i.UserId equals ii.UserId
                select new FriendShipResBase(i.Id.Value.ToString(),i.FriendId, ii.FullName, ii.AvatarUrl);
            var response = new PaginatedResult<FriendShipResBase>(paginationRequest.PageIndex, paginationRequest.PageSize, friendShips.Count(), friendShips.ToList());
            await cacheService.SetCacheAsync(cacheKey, response, TimeSpan.FromMinutes(10));
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<FriendShipResBase>> GetFriendShipByUserIdsAsync(IEnumerable<string> userId, CancellationToken cancellationToken = default)
    {
        try
        {

            var friendShips = await repository.Table<FriendShip>()
                .Select(f => new
                {
                    Id = f.Id,
                    FriendId = f.FriendId,
                    UserId = f.UserId
                })
                .Where(f => userId.Contains(f.UserId) || userId.Contains(f.FriendId))
                .ToListAsync(cancellationToken);
            
            var friendInfos = await repository.Table<UserInfo>().Select(r => new
            {
                UserId = r.UserId,
                FullName = r.FullName,
                AvatarUrl = r.AvatarUrl
            }).Where(r => friendShips.Select(r => r.FriendId).Contains(r.UserId)).ToListAsync(cancellationToken);
            
            var dataMapper = from i in friendShips
                join ii in friendInfos on i.FriendId equals ii.UserId
                select new FriendShipResBase(i.Id.Value.ToString(), i.FriendId, ii.FullName, ii.AvatarUrl);
            
            return dataMapper;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}