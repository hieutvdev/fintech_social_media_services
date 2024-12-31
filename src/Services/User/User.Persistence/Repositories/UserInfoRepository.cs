using System.Text;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.Cache;
using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using Newtonsoft.Json;
using Serilog;
using User.Application.DTOs.Request.UserInfo;
using User.Application.DTOs.Response.UserInfo;
using User.Application.Exceptions;
using User.Application.Repositories;
using User.Domain.Models;
using User.Domain.ValuesObjects;
using User.Infrastructure.Configuration;
using User.Persistence.Data;

namespace User.Persistence.Repositories;

public class UserInfoRepository
(IRepositoryBaseService<ApplicationDbContext> repositoryBaseService, IDistributedCacheService cache, IAuthorizeExtension authorizeExtension)
: IUserInfoRepository

{
    public async Task<bool> CreateUserInfoAsync(CreateUserInfoReqDto payload, CancellationToken cancellationToken = default)
    {
        var userInfoId = UserInfoId.Of(Guid.NewGuid());
        try
        {   
            var userInfo = UserInfo.Create(userInfoId, payload.UserId, null, payload.Gender, null, null, null, null, null, 1, null);
            await repositoryBaseService.AddAsync(userInfo, cancellationToken);
            var isSuccessful = await repositoryBaseService.SaveChangesAsync(cancellationToken) > 0;
            return isSuccessful;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateUserInfoAsync(UpdateUserInfoReqDto payload, CancellationToken cancellationToken = default)
    {
        var userInfoId = UserInfoId.Of(Guid.Parse(payload.Id));
        try
        {
            var isUpdated = await repositoryBaseService.ExecuteUpdateAsync<UserInfo>(
                condition: r => r.Id == userInfoId,
                updateExpression: updates => updates
                    .SetProperty(r => r.CoverPhoto, payload.CoverPhoto)
                    .SetProperty(r => r.Gender, payload.Gender)
                    .SetProperty(r => r.Bio, payload.Bio)
                    .SetProperty(r => r.CurrentCity, payload.CurrentCity)
                    .SetProperty(r => r.Education, payload.Education)
                    .SetProperty(r => r.Work, payload.Work)
                    .SetProperty(r => r.Skills, payload.Skills)
                    .SetProperty(r => r.RelationshipStatus, payload.RelationshipStatus)
                    .SetProperty(r => r.Hobbies, payload.Hobbies),
                cancellationToken: cancellationToken
            );

            await cache.RemoveCacheAsync(string.Format(CacheKey.UserInfo.GetDetail, payload.Id));

            return isUpdated;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<UserInfoDetailResBaseDto> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var userGetInfo = authorizeExtension.GetUserFromClaimToken().Id;

            var cacheValue = await cache.GetCacheAsync(string.Format(CacheKey.UserInfo.GetDetail, userGetInfo));    
            Log.Information(cacheValue);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                
                var resCache =  JsonConvert.DeserializeObject<UserInfoDetailResBaseDto>(cacheValue)!;
                
                Log.Warning("Cache hit for user info detail.");
                Log.Warning(resCache.ToString());

                return resCache;
            }
            var userInfoFound =
                await repositoryBaseService.FirstOrDefaultAsNoTrackingAsync<UserInfo>(r => r.UserId == userGetInfo,
                    cancellationToken);

            if (userInfoFound == null)
            {
                throw new UserInfoNotFoundException("User info not found.");
            }
            
            var userInfoDetailResBaseDto = new UserInfoDetailResBaseDto(
                userInfoFound.Id.Value.ToString(),
                userInfoFound.UserId,
                userInfoFound.CoverPhoto!,
                userInfoFound.Gender,
                userInfoFound.Bio!,
                userInfoFound.CurrentCity!,
                userInfoFound.Education!,
                userInfoFound.Work!.Split("||").ToList(),
                userInfoFound.Skills!.Split("||").ToList(),
                userInfoFound.Hobbies!.Split("||").ToList(),
                "1",
                1);
            
            await cache.SetCacheAsync(string.Format(CacheKey.UserInfo.GetDetail, userGetInfo), userInfoDetailResBaseDto, TimeSpan.FromMinutes(5));
            return userInfoDetailResBaseDto;
            
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}