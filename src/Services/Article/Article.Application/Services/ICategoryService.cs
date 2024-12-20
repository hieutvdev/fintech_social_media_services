using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Domain.Models;
using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;

namespace Article.Application.Services;

public interface ICategoryService
{
    Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default!);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default!);
    Task<IEnumerable<CategoryGetSelectResponseDto>> GetSelectAsync(CancellationToken cancellationToken = default!);
    Task<PaginatedResult<Category>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);

    Task<bool> DeleteAsync(DeleteCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default!);

    Task<bool> UpdateAsync(UpdateCategoryRequestDto categoryRequestDto,CancellationToken cancellationToken = default!);
    
    Task<Category> GetDetailsAsync(string id, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<CategoryResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default!);

}