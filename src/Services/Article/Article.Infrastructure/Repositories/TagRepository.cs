using Article.Application.DTOs.Request.Tag;
using Article.Application.DTOs.Response.Tag;
using Article.Application.Exceptions;
using Article.Application.Mappers.TagMapper;
using Article.Application.Repositories;
using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Article.Infrastructure.Data;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using BuildingBlocks.Security;
using Microsoft.EntityFrameworkCore;
using ShredKernel.BaseClasses;
using ShredKernel.Specifications;
using QueryableExtensions = ShredKernel.Specifications.QueryableExtensions;


namespace Article.Infrastructure.Repositories;

public class TagRepository(IAuthorizeExtension authorizeExtension, IRepositoryService<string, Tag, ApplicationDbContext> repositoryService, IMapper mapper) : ITagRepository
{
    public async Task<bool> CreateAsync(CreateTagRequestDto createTagRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var tagId = TagId.Of(Guid.NewGuid());
            mapper.Map<Tag>(createTagRequestDto);
            var tag = Tag.Create(tagId, createTagRequestDto.Name);
            tag.CreatedBy = authorizeExtension.GetUserFromClaimToken().Id;
            
            await repositoryService.AddAsync(tag, cancellationToken);
            var res = await repositoryService.SaveChangeAsync(cancellationToken) > 0;
            return res;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await repositoryService.GetSelectorAsync<SelectListItem>(
                s => new SelectListItem(s.Id.Value.ToString(), s.Name), cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<Tag>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteAsync(DeleteTagRequestDto deleteTagRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var tags =
                await repositoryService.WhereAsync(r => deleteTagRequestDto.Ids.Contains(r.Id.Value.ToString()),
                    cancellationToken);

            if (tags.Count() != deleteTagRequestDto.Ids.Length)
            {
                throw new BadRequestException("Cannot be to find delete data");
            }
            repositoryService.DeleteAsync(tags, cancellationToken);
            var responseDelete = await repositoryService.SaveChangeAsync(cancellationToken) > 0;

            return responseDelete;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<bool> UpdateAsync(UpdateTagRequestDto updateTagRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var tagId = TagId.Of(Guid.Parse(updateTagRequest.Id));
            var tagFound =
                await repositoryService.FirstOrDefaultAsync(r => r!.Id == tagId,
                    cancellationToken) ?? throw new TagNotFoundException("Cannot be to find tag");

            TagMapperDto.UpdateCategoryWithNewValues(tagFound, updateTagRequest);

            repositoryService.Update(tagFound);

            var response = await repositoryService.SaveChangeAsync(cancellationToken) > 0;
            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<Tag> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var tagId = TagId.Of(Guid.Parse(id));

            var tagFound = await repositoryService.FirstOrDefaultAsync(c => c.Id == tagId, cancellationToken);
            if (tagFound == null)
            {
                throw new TagNotFoundException("Cannot to find tag with that ID");
            }

            return tagFound;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PaginatedResult<TagResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tagBases = await repositoryService.Table()
                .Select(r => new { r.Id, r.Name, r.Slug }) // Chỉ lấy các cột cần thiết từ database
                .WhereIf(!string.IsNullOrEmpty(searchBaseModel.KeySearch),
                    r => r.Name.ToLower().Contains(searchBaseModel.KeySearch!.ToLower()))
                .ToListAsync(cancellationToken);

            var tags = tagBases.Select(tag => new TagResponseBaseDto(
                tag.Id.Value.ToString(),
                tag.Name,
                tag.Slug
            )).ToList();

            
            if (!string.IsNullOrEmpty(searchBaseModel.SortBy))
            {
                var sortBy = QueryableExtensions.GetPropertyGetter<TagResponseBaseDto>(searchBaseModel.SortBy);
                tags = searchBaseModel.IsAscending 
                    ? tags.OrderBy(sortBy).ToList()
                    : tags.OrderByDescending(sortBy).ToList();
            }
            
            long totalRecords = tags.Count();
            var dataResponse = tags.Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize);

            PaginatedResult<TagResponseBaseDto> response =
                new PaginatedResult<TagResponseBaseDto>(paginationRequest.PageIndex, paginationRequest.PageSize,
                    totalRecords, dataResponse);

            return response;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}