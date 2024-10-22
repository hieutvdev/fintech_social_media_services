using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Repositories;
using Article.Application.Services;
using Article.Domain.Models;
using BuildingBlocks.Pagination;

namespace Article.Infrastructure.Services;

public class CategoryService (ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.CreateAsync(createCategoryRequestDto, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await categoryRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<CategoryGetSelectResponseDto>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        return await categoryRepository.GetSelectAsync(cancellationToken);
    }

    public async Task<PaginatedResult<Category>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.GetPageAsync(paginationRequest, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.DeleteAsync(categoryRequestDto, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.UpdateAsync(categoryRequestDto, cancellationToken);
    }

    public async Task<Category> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.GetDetailsAsync(id, cancellationToken);
    }
}