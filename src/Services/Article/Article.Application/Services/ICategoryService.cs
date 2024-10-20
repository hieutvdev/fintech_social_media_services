using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;

namespace Article.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default!);
    Task<IEnumerable<CategoryResponseBaseDto>> GetAllAsync(CancellationToken cancellationToken = default!);
}