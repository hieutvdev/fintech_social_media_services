using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Domain.Models;

namespace Article.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default!);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default!);
}