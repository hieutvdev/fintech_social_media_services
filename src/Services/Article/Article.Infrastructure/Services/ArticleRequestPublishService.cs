using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.DTOs.Response.ArticleRequestPublish;
using Article.Application.Repositories;
using Article.Application.Services;
using BuildingBlocks.Pagination;

namespace Article.Infrastructure.Services;

public class ArticleRequestPublishService(IArticleRequestPublishRepository repository) : IArticleRequestPublishService
{
    public async Task<bool> CreateAsync(CreateArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.CreateAsync(payload, cancellationToken);
    }

    public async Task<IEnumerable<ArticleRequestPublishResBaseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repository.GetAllAsync(cancellationToken);
    }

    public async Task<ArticleReqPublishDetailResDto> DetailAsync(string id, CancellationToken cancellationToken = default)
    {
        return await repository.DetailAsync(id, cancellationToken);
    }

    public async Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetListAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default)
    {
        return await repository.GetListAsync(query, cancellationToken);
    }

    public async Task<PaginatedResult<ArticleRequestPublishResBaseDto>> GetByUserAsync(ArticleReqSearchListDto query, CancellationToken cancellationToken = default)
    {
        return await repository.GetByUserAsync(query, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.DeleteAsync(payload, cancellationToken);
    }

    public async Task<bool> UpdateAsync(UpdateArticleRequestPublishDto payload, CancellationToken cancellationToken = default)
    {
        return await repository.UpdateAsync(payload, cancellationToken);
    }
}