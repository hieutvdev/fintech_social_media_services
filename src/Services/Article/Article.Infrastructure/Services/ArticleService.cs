using Article.Application.DTOs.Request.Article;
using Article.Application.DTOs.Response.Article;
using Article.Application.Repositories;
using Article.Application.Services;
using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;

namespace Article.Infrastructure.Services;

public class ArticleService(IArticleRepository repo)
: IArticleService
{
    public async Task<bool> CreateAsync(CreateArticleRequestDto createArticleRequest, CancellationToken cancellationToken = default)
    {
        return await repo.CreateAsync(createArticleRequest, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Models.Article>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await repo.GetAllAsync(cancellationToken);
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectAsync(CancellationToken cancellationToken = default)
    {
        return await repo.GetSelectAsync(cancellationToken);
    }

    public async Task<PaginatedResult<ArticleResponseBaseDto>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        return await repo.GetPageAsync(paginationRequest, cancellationToken);
    }

    public async Task<PaginatedResult<ArticleResponseBaseDto>> GetListAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        return await repo.GetListAsync(paginationRequest, searchBaseModel, cancellationToken);
    }

    public async Task<Domain.Models.Article> GetDetailAsync(string id, CancellationToken cancellationToken = default)
    {
        return await repo.DetailAsync(id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteArticleRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        return await repo.DeleteAsync(requestDto, cancellationToken);   
    }

    public async Task<bool> UpdateAsync(UpdateArticleRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        return await repo.UpdateAsync(requestDto, cancellationToken);
    }

    public async Task<ArticleDetailRelateResponseDto> GetRelateDetails(string id, CancellationToken cancellationToken = default)
    {
        return await repo.GetRelateDetails(id, cancellationToken);
    }

    public async Task<PaginatedResult<ArticleByUserResponseDto>> GetArticleByUserAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
        return await repo.GetArticleByUserAsync(paginationRequest, searchBaseModel, cancellationToken);
    }

    public async Task<PaginatedResult<ArticleByAdminResponseDto>> GetArticleByAdminAsync(PaginationRequest paginationRequest, SearchBaseModel searchBaseModel,
        CancellationToken cancellationToken = default)
    {
       return await repo.GetArticleByAdminAsync(paginationRequest, searchBaseModel, cancellationToken);
    }

    public async Task<bool> SendRequestArticleByUserAsync(SendRequestArticleRequestDto payload, CancellationToken cancellationToken = default)
    {
        return await repo.SendRequestArticleByUserAsync(payload, cancellationToken);
    }

    public async Task<bool> ConfirmRequestArticleByAdminAsync(ConfirmRequestArticleRequestDto payload,
        CancellationToken cancellationToken = default)
    {
        return await repo.ConfirmRequestArticleByAdminAsync(payload, cancellationToken);
    }

    public async Task<IEnumerable<ArticleRelateResponseDto>> GetArticleRelateByCateNameAsync(string categoryNames, CancellationToken cancellationToken = default)
    {
        return await repo.GetArticleRelateByIdAsync(categoryNames, cancellationToken);
    }
}