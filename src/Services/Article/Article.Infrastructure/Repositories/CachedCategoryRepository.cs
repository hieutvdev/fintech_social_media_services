using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Repositories;
using Article.Domain.Models;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.Cache;
using Newtonsoft.Json;
using ShredKernel.BaseClasses;

namespace Article.Infrastructure.Repositories;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly IDistributedCacheService _distributedCacheService;
    private readonly ICategoryRepository _categoryRepository;

    public CachedCategoryRepository(IDistributedCacheService distributedCacheService, ICategoryRepository categoryRepository)
    {
        _distributedCacheService = distributedCacheService;
        _categoryRepository = categoryRepository;
    }
    public Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string cacheKey = "Category_GetAll";
        var cachedData = await _distributedCacheService.GetCacheAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(cachedData); // Sử dụng Newtonsoft.Json
        }

        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        // Serialize dữ liệu trước khi lưu cache
        var serializedData = JsonConvert.SerializeObject(categories);
        await _distributedCacheService.SetCacheAsync(cacheKey, serializedData, TimeSpan.FromMinutes(10));

        return categories;
    }
    public Task<IEnumerable<CategoryGetSelectResponseDto>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Category>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(DeleteCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(UpdateCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<CategoryResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}