using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Repositories;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository(IRepositoryServiceV2 repositoryServiceV2, IMapper mapper)
: ICategoryRepository
{
    public async Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var categoryId = CategoryId.Of(Guid.NewGuid());
            mapper.Map<Category>(createCategoryRequestDto);
            var category = Category.Create(categoryId, createCategoryRequestDto.Name, createCategoryRequestDto.Description);
            var createCategory = await repositoryServiceV2.AddAsync(category, cancellationToken);
            var res = await repositoryServiceV2.SaveChangesAsync(cancellationToken) > 0;
            return res;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<CategoryResponseBaseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await repositoryServiceV2.Table<Category>().Select(r => mapper.Map<CategoryResponseBaseDto>(r)).ToListAsync(cancellationToken);

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}