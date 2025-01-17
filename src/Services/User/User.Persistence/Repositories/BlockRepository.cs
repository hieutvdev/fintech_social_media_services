using User.Application.DTOs.Request.Block;
using User.Application.DTOs.Response.Block;

namespace User.Persistence.Repositories;

public class BlockRepository
(IAuthorizeExtension authorizeExtension, IRepositoryBaseService<ApplicationDbContext> repository, IDistributedCacheService cacheService)
: IBlockRepository
{
    public async Task<bool> CreateAsync(CreateBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var blockId = BlockId.Of(Guid.NewGuid());
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var block = Block.Create(blockId, userId, payload.BlockedId);
            await repository.AddAsync(block, cancellationToken);
            var isSuccessful = await repository.SaveChangesAsync(cancellationToken) > 0;
            if (isSuccessful)
            {
                await cacheService.RemoveCacheAsync(string.Format(CacheKey.Block.GetListByUserLogin, userId));
                
            }

            return isSuccessful;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteBlockReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var blockIds = payload.BlockedIds.Select(r => BlockId.Of(Guid.Parse(r))).ToList();
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var isDeleted =
                await repository.ExecuteDeleteAsync<User.Domain.Models.Block>(r => blockIds.Contains(r.Id),
                    cancellationToken);
            if (isDeleted)
            {
                await cacheService.RemoveCacheAsync(string.Format(CacheKey.Block.GetListByUserLogin, userId));
            }

            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<BlockResBaseDto>> GetBlocksByUserLoginAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var userId = authorizeExtension.GetUserFromClaimToken().Id;
            var cacheKey = $"{string.Format(CacheKey.Block.GetListByUserLogin, userId)}{CachingHelper.ObjectToQueryString(paginationRequest)}";
            if (!string.IsNullOrEmpty(cacheKey))
            {
                var cacheValue = await cacheService.GetCacheAsync(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return JsonConvert.DeserializeObject<PaginatedResult<BlockResBaseDto>>(cacheValue) ?? new PaginatedResult<BlockResBaseDto>(paginationRequest.PageIndex, paginationRequest.PageSize, 0, new List<BlockResBaseDto>());
                }
            }

            var blocks = await repository.Table<User.Domain.Models.Block>().OrderBy(r => r.CreatedAt).Select(r => new
            {
                Id = r.Id,
                BlockedId = r.BlockedId,
                UserId = r.BlockerId
            }).Where(r => r.UserId == userId).Skip((paginationRequest.PageIndex -1)*paginationRequest.PageSize).Take(paginationRequest.PageSize).ToListAsync(cancellationToken);

            var userDates = from i in blocks
                join ii in repository.Table<UserInfo>().Select(r => new
                {
                    UserId = r.UserId,
                    FullName = r.FullName,
                    Avatar = r.AvatarUrl
                }) on i.BlockedId equals ii.UserId
                select new BlockResBaseDto(i.Id.Value.ToString(), ii.FullName, ii.Avatar);

            var blockResBaseDtos = userDates as BlockResBaseDto[] ?? userDates.ToArray();
            var response = new PaginatedResult<BlockResBaseDto>(paginationRequest.PageIndex, paginationRequest.PageSize, blockResBaseDtos.Count(), blockResBaseDtos.ToList());
            
            await cacheService.SetCacheAsync(cacheKey, JsonConvert.SerializeObject(response), TimeSpan.FromMinutes(20));
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> CheckBlockAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentUserId = authorizeExtension.GetUserFromClaimToken().Id;
            
            var isBlocked = await repository.CountAsync<User.Domain.Models.Block>(r => r.BlockedId == userId && r.BlockerId == currentUserId, cancellationToken) > 0;
                
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}