using Article.Application.DTOs.Request.Tag;
using Article.Application.DTOs.Response.Tag;
using Article.Application.Repositories;
using Article.Application.Services;
using Article.Domain.Models;
using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;

namespace Article.Infrastructure.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    public async Task<bool> CreateAsync(CreateTagRequestDto createTagRequestDto, CancellationToken cancellationToken = default)
    {
        return await tagRepository.CreateAsync(createTagRequestDto, cancellationToken);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetSelectAsync(cancellationToken);
    }

    public async Task<PaginatedResult<Tag>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetPageAsync(paginationRequest, cancellationToken);
    }
    

    public async Task<bool> DeleteAsync(DeleteTagRequestDto deleteTagRequestDto, CancellationToken cancellationToken = default)
    {
        return await tagRepository.DeleteAsync(deleteTagRequestDto, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateTagRequestDto updateTagRequest, CancellationToken cancellationToken = default)
    {
        return await tagRepository.UpdateAsync(updateTagRequest, cancellationToken);
    }
    

    public async Task<Tag> GetDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetDetailsAsync(id, cancellationToken);
    }

    public async Task<PaginatedResult<TagResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        return await tagRepository.GetListAsync(paginationRequest, searchBaseModel, cancellationToken);
    }
}