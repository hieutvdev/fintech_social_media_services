using Article.Application.DTOs.Request.Tag;
using Article.Application.DTOs.Response.Tag;

namespace Article.Application.Services;

public interface ITagService
{
    Task<bool> CreateAsync(CreateTagRequestDto createTagRequestDto, CancellationToken cancellationToken = default!);
    Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default!);
    Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default!);
    Task<PaginatedResult<Tag>> GetPageAsync( PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);
    Task<bool> DeleteAsync(DeleteTagRequestDto deleteTagRequestDto, CancellationToken cancellationToken = default!);
    Task<bool> UpdateAsync(UpdateTagRequestDto updateTagRequest,CancellationToken cancellationToken = default!);
    Task<Tag> GetDetailsAsync(string id, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<TagResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default!);
}