using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.DTOs.Response.ArticleRequestPublish;

namespace Article.Application.Services;

public interface IArticleRequestPublishService
{
    Task<bool> CreateAsync(CreateArticleRequestPublishDto payload,CancellationToken cancellationToken = default!);
    Task<IEnumerable<ArticleRequestPublishResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default!);
    
    Task<ArticleReqPublishDetailResDto> DetailAsync(string id, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetListAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default!);
    
    Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetByUserAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default!);
    
    Task<bool> DeleteAsync(DeleteArticleRequestPublishDto payload, CancellationToken cancellationToken = default!);
    
    Task<bool> UpdateAsync(UpdateArticleRequestPublishDto payload, CancellationToken cancellationToken = default!);
}