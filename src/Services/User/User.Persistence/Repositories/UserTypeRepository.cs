using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.Cache;
using BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using ShredKernel.BaseClasses;
using ShredKernel.Specifications;
using User.Application.DTOs.Request.UserType;
using User.Application.DTOs.Response.UserType;
using User.Application.Exceptions;
using User.Application.Repositories;
using User.Application.UseCases.Models.UserType;
using User.Domain.Models;
using User.Infrastructure.Configuration;
using User.Persistence.Data;

namespace User.Persistence.Repositories;

public class UserTypeRepository(IRepositoryBaseService<ApplicationDbContext> repository,IDistributedCacheService cacheService, IAuthorizeExtension authorizeExtension)
: IUserTypeRepository
{
    public async Task<IEnumerable<UserType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Log.Information("Attempting to retrieve UserType from cache.");
            var cacheValue = await cacheService.GetCacheAsync(CacheKey.UserType.GetAll);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                Log.Information("UserType retrieved from cache.");
                return JsonConvert.DeserializeObject<IEnumerable<UserType>>(cacheValue) ?? new List<UserType>();
            }

            Log.Information("Cache is empty, retrieving UserType from database.");
            var result = await repository.Table<UserType>().AsNoTracking().ToListAsync(cancellationToken);
            await cacheService.SetCacheAsync(CacheKey.UserType.GetAll, result, TimeSpan.FromHours(3));
            Log.Information("UserType retrieved from database and cached.");
            return result;
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while retrieving UserType.");
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<UserType> DetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.FirstOrDefaultAsNoTrackingAsync<UserType>(r => r.Id == id, cancellationToken);
            if (result is null)
            {
                throw new UserTypeNotFoundException("User type not found");
            }

            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<UserTypeResBaseDto>> GetListAsync(UserTypeSearchListModelQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.Yield();
            var baseRequest = repository.Table<UserType>().Select(r => new
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).WhereIf(!string.IsNullOrEmpty(query.KeySearch),
                r => r.Name.ToLower().Contains(query.KeySearch!.ToLower()) ||
                     r.Description.ToLower().Contains(query.KeySearch!.ToLower()));


            var resultDto = baseRequest.Select(r => new UserTypeResBaseDto(r.Id, r.Name, r.Description)).AsEnumerable();

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<UserTypeResBaseDto>(query.SortBy);

                resultDto = query.IsAscending == true ? resultDto.OrderBy(sortBy) : resultDto.OrderByDescending(sortBy);
            }
            else
            {
                resultDto = resultDto.OrderBy(r => r.Id);
            }
            
            var resultDtoToList = resultDto.ToList();
            var totalRecords = resultDtoToList.Count();

            var paginationRequest = new PaginationRequest(query.PageIndex ?? 1, query.PageSize ?? 20);
            var paginatedResult = resultDtoToList.Skip((paginationRequest.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize);

            return new PaginatedResult<UserTypeResBaseDto>(query.PageIndex ?? 1, query.PageSize ?? 20, totalRecords,
                paginatedResult);

        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repository.Table<UserType>().Select(r => new
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync(cancellationToken);

            var selectListItems = result.Select(r => new SelectListItem(r.Id.ToString(), r.Name)).ToList();
            return selectListItems;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> CreateAsync(CreateUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var userType = UserType.Create(payload.Name, payload.Description);
            userType.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
            await repository.AddAsync(userType, cancellationToken);
            var result = await repository.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var userTypeFound =
                await repository.FirstOrDefaultAsNoTrackingAsync<UserType>(r => r.Id == payload.Id, cancellationToken);
            if (userTypeFound is null)
            {
                throw new UserTypeNotFoundException("User type not found");   
            }
            userTypeFound.Update(payload.Name, payload.Description);
            userTypeFound.UpdatedBy = authorizeExtension.GetUserFromClaimToken().Id;
            repository.Update(userTypeFound);
            var result = await repository.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteUserTypeReqDto payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var IdUnique = payload.Ids.Distinct().ToList();
            var isDeleted = await repository.ExecuteDeleteAsync<UserType>(r => IdUnique.Contains(r.Id), cancellationToken);
            return isDeleted;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}