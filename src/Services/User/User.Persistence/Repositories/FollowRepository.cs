using SecurityDriven;
using User.Application.DTOs.Request.Follow;
using User.Application.DTOs.Response.Follow;
using User.Application.UseCases.Models.Follow;

namespace User.Persistence.Repositories;

public class FollowRepository(IRepositoryBaseService<ApplicationDbContext> repository, IAuthorizeExtension authorize, IDistributedCacheService cacheService) : IFollowRepository
{
    public async Task<bool> CreateAsync(CreateFollowReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var followId = FollowId.Of(FastGuid.NewGuid());
            string userCreate = authorize.GetUserFromClaimToken().Id;
            Follow follow = Follow.Create(followId, userCreate, payload.UserId);
            await repository.AddAsync(follow, cancellationToken);
            var isSuccess = await repository.SaveChangesAsync(cancellationToken) > 0;
            return isSuccess;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteFollowReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var followIds = payload.UserIds.Select(r => FollowId.Of(Guid.Parse(r))).ToList();
            var isDeleted =
                await repository.ExecuteDeleteAsync<Follow>(condition: r => followIds.Contains(r.Id),
                    cancellationToken);
            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> CheckFollowAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userLogin = authorize.GetUserFromClaimToken().Id;
            var isFollowing = await repository.Table<Follow>().Select(r => new
            {
                FollowerId = r.FollowerId,
                FollowingId = r.FollowingId
            }).Where(r => r.FollowerId == userLogin && r.FollowingId == userId).AnyAsync(cancellationToken);
            return isFollowing;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FollowResBaseDto>> GetListFollowAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var queryable = repository.Table<Follow>().Select(r => new
            {
                Id = r.Id,
                FollowerId = r.FollowerId,
                FollowingId = r.FollowingId
            });


            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    // public async Task<PaginatedResult<FollowResForUserDto>> GetListFollowerForUserLoginAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    // {
    //     try
    //     {
    //         var userLogin = authorize.GetUserFromClaimToken().Id;
    //         var queryable = from i in repository.Table<Follow>()
    //                 .Select(r => new
    //                 {
    //                     Id = r.Id,
    //                     FollowerId = r.FollowerId,
    //                     FollowingId = r.FollowingId
    //                 }).Where(r => r.FollowingId == userLogin)
    //             join ii in repository.Table<UserInfo>().Select(r => new
    //             {
    //                 UserId = r.UserId,
    //                 FullName = r.FullName,
    //                 AvatarUrl = r.AvatarUrl
    //             }) on i.FollowerId equals ii.UserId into g
    //             from ii in g.DefaultIfEmpty()
    //             select new
    //             {
    //                 Id = i.Id,
    //                 UserId = ii.UserId,
    //                 FullName = ii.FullName,
    //                 AvatarUrl = ii.AvatarUrl
    //             };
    //
    //         if (!string.IsNullOrEmpty(query.KeySearch))
    //         {
    //             queryable = from i in queryable
    //                 where i.FullName.Contains(query.KeySearch)
    //                 select i;
    //         }
    //
    //         var resDto = queryable.Select(r => new FollowResForUserDto(r.Id.Value.ToString(), r.UserId, r.FullName, r.AvatarUrl)).AsEnumerable();
    //
    //         if (!string.IsNullOrEmpty(query.SortBy))
    //         {
    //             var sortBy = QueryableExtensions.GetPropertyGetter<FollowResForUserDto>(query.SortBy);
    //             resDto = query.IsAscending == true
    //                 ? resDto.OrderBy(sortBy)
    //                 : resDto.OrderByDescending(sortBy);
    //         }
    //         else
    //         {
    //             resDto = resDto.OrderBy(r => r.Id);
    //         }
    //
    //         var followResForUserDtos = resDto.ToList();
    //         int totalRecords =  followResForUserDtos.Count();
    //         PaginationRequest paginationRequest = new PaginationRequest(query.PageIndex ?? 1, query.PageSize ?? 20);
    //         IEnumerable<FollowResForUserDto> data = followResForUserDtos.Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
    //             .Take(paginationRequest.PageSize);
    //         
    //         var response = new PaginatedResult<FollowResForUserDto>(query.PageIndex ?? 1, query.PageSize ?? 20,
    //             totalRecords,
    //             data);
    //
    //         return response;
    //     }
    //     catch (Exception e)
    //     {
    //         throw new BadRequestException(e.Message);
    //     }
    // }

    
    public async Task<PaginatedResult<FollowResForUserDto>> GetListFollowerForUserLoginAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var userLogin = authorize.GetUserFromClaimToken().Id;

            var queryable = from i in repository.Table<Follow>()
                            .Where(r => r.FollowingId == userLogin)
                            .Select(r => new
                            {
                                Id = r.Id,
                                FollowerId = r.FollowerId,
                                FollowingId = r.FollowingId
                            })
                            join ii in repository.Table<UserInfo>()
                            .Select(r => new
                            {
                                UserId = r.UserId,
                                FullName = r.FullName,
                                AvatarUrl = r.AvatarUrl
                            }) on i.FollowerId equals ii.UserId into g
                            from ii in g.DefaultIfEmpty()
                            select new FollowResForUserDto 
                            (
                                i.Id.Value.ToString(),
                                ii.UserId,
                                ii.FullName,
                                ii.AvatarUrl
                            );

            if (!string.IsNullOrEmpty(query.KeySearch))
            {
                queryable = queryable.Where(r => r.FullName.Contains(query.KeySearch));
            }

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetterV2<FollowResForUserDto>(query.SortBy);
                queryable = query.IsAscending == true
                    ? queryable.OrderBy(sortBy)
                    : queryable.OrderByDescending(sortBy);
            }
            else
            {
                queryable = queryable.OrderBy(r => r.Id);
            }

            // Count total records before pagination
            int totalRecords = await queryable.CountAsync(cancellationToken);

            // Apply pagination
            var paginationRequest = new PaginationRequest(query.PageIndex ?? 1, query.PageSize ?? 20);
            var data = await queryable
                .Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);

            var response = new PaginatedResult<FollowResForUserDto>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalRecords,
                data);

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
    public async Task<PaginatedResult<FollowResForUserDto>> GetListFollowingForUserLoginAsync(
        FollowSearchListModelQuery query,
        CancellationToken cancellationToken = default!)
    {
        try
        {
            throw new NotSupportedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<FollowResForOtherUser>> GetListFollowForOtherUserAsync(FollowSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> ConvertFollowToFriendAsync(ConvertFollowToFriendReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}