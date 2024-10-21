using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Repositories;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository(IRepositoryService<string, Category, ApplicationDbContext> repositoryService, IMapper mapper)
: ICategoryRepository
{
    public async Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var categoryId = CategoryId.Of(Guid.NewGuid());
            mapper.Map<Category>(createCategoryRequestDto);
            var category = Category.Create(categoryId, createCategoryRequestDto.Name, createCategoryRequestDto.Description);
            await repositoryService.AddAsync(category, cancellationToken);
            var res = await repositoryService.SaveChangeAsync(cancellationToken) > 0;
            return res;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await repositoryService.GetAllAsync(cancellationToken);

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}