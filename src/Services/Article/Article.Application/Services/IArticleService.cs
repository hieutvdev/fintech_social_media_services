using Article.Application.DTOs.Request.Article;
using Article.Application.DTOs.Response.Article;

namespace Article.Application.Services;

public interface IArticleService
{
    Task<bool> CreateAsync(CreateArticleRequestDto createArticleRequest, CancellationToken cancellationToken = default!);
    Task<IEnumerable<Domain.Models.Article>> GetAllAsync(CancellationToken cancellationToken = default!);
    Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default!);
    Task<PaginatedResult<ArticleResponseBaseDto>> GetPageAsync( PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<ArticleResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel, CancellationToken cancellationToken = default!);
    
    Task<Domain.Models.Article> GetDetailAsync(string id, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteAsync(DeleteArticleRequestDto requestDto, CancellationToken cancellationToken = default!);

    Task<bool> UpdateAsync(UpdateArticleRequestDto requestDto, CancellationToken cancellationToken = default!);

    Task<ArticleDetailRelateResponseDto> GetRelateDetails(string id, CancellationToken cancellationToken = default!);
    Task<PaginatedResult<ArticleByUserResponseDto>> GetArticleByUserAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<ArticleByAdminResponseDto>> GetArticleByAdminAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default!);
    
    Task<bool> SendRequestArticleByUserAsync(SendRequestArticleRequestDto payload, CancellationToken cancellationToken = default!);
    Task<bool> ConfirmRequestArticleByAdminAsync(ConfirmRequestArticleRequestDto payload, CancellationToken cancellationToken = default!);
    Task<IEnumerable<ArticleRelateResponseDto>> GetArticleRelateByCateNameAsync(string categoryNames, CancellationToken cancellationToken = default!);
}