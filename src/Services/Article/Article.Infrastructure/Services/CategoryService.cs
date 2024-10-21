using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Repositories;
using Article.Application.Services;
using Article.Domain.Models;

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
}