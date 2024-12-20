using Article.Application.DTOs.Request.Category;
using Article.Application.DTOs.Response.Category;
using Article.Application.Exceptions;
using Article.Application.Mappers.CategoryMapper;
using Article.Application.Repositories;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using BuildingBlocks.Security;
using JasperFx.Core;
using Microsoft.EntityFrameworkCore;
using ShredKernel.BaseClasses;
using ShredKernel.Specifications;

namespace Article.Infrastructure.Repositories;

public class CategoryRepository(IRepositoryService<string, Category, ApplicationDbContext> repositoryService, IMapper mapper, IAuthorizeExtension authorizeExtension)
: ICategoryRepository
{
    public async Task<bool> CreateAsync(CreateCategoryRequestDto createCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var categoryId = CategoryId.Of(Guid.NewGuid());
            mapper.Map<Category>(createCategoryRequestDto);
            var category = Category.Create(categoryId, createCategoryRequestDto.Name, createCategoryRequestDto.Description);
            category.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
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

    public async Task<IEnumerable<CategoryGetSelectResponseDto>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repositoryService.GetSelectorAsync<CategoryGetSelectResponseDto>(
                s => new CategoryGetSelectResponseDto(s.Id.Value.ToString(), s.Name, s.Slug), cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<Category>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var paginatedResult =
                await repositoryService.GetPageAsync(paginationRequest, null, cancellationToken);
            return paginatedResult;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> DeleteAsync(DeleteCategoryRequestDto categoryRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var categories =
                await repositoryService.WhereAsync(r => categoryRequestDto.ids.Contains(r.Id.Value.ToString()),
                    cancellationToken);

            if (categories.Count() != categoryRequestDto.ids.Length)
            {
                throw new BadRequestException("Cannot be to find delete data");
            }
            repositoryService.DeleteAsync(categories, cancellationToken);
            var responseDelete = await repositoryService.SaveChangeAsync(cancellationToken) > 0;

            return responseDelete;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateCategoryRequestDto updateCategoryRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var categoryFound =
                await repositoryService.FirstOrDefaultAsync(r => r.Id.Value.ToString() == updateCategoryRequestDto.Id,
                    cancellationToken) ?? throw new CategoryNotFoundException("Cannot be to find category");

            CategoryMapperDto.UpdateCategoryWithNewValues(categoryFound, updateCategoryRequestDto);

            repositoryService.Update(categoryFound);

            var response = await repositoryService.SaveChangeAsync(cancellationToken) > 0;
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<Category> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var parsedId = CategoryId.Of(Guid.Parse(id)); 

            var categoryFound = await repositoryService.FirstOrDefaultAsync(c => c.Id == parsedId, cancellationToken)
                                ?? throw new CategoryNotFoundException("Cannot find category");
            
            return categoryFound;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<CategoryResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest,SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await repositoryService.Table()
                .Select(r => r.CategoryToBaseDto())
                .WhereIf(!string.IsNullOrEmpty(searchBaseModel.KeySearch),r => r.Name.ToLower().Contains(searchBaseModel.KeySearch!))
                .ToListAsync(cancellationToken);

            if (!string.IsNullOrEmpty(searchBaseModel.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<CategoryResponseBaseDto>(searchBaseModel.SortBy);
                categories = searchBaseModel.IsAscending 
                    ? categories.OrderBy(sortBy).ToList()
                    : categories.OrderByDescending(sortBy).ToList();
            }
            
            long totalRecords = categories.Count();
            var dataResponse = categories.Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize);

            PaginatedResult<CategoryResponseBaseDto> response =
                new PaginatedResult<CategoryResponseBaseDto>(paginationRequest.PageIndex, paginationRequest.PageSize,
                    totalRecords, dataResponse);

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
        
    }
    
}