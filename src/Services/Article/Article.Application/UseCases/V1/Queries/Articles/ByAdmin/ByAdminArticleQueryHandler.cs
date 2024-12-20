using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.ByAdmin;

public class ByAdminArticleQueryHandler(IArticleService service) : IQueryHandler<ByAdminArticleQuery, PaginatedResult<ArticleByAdminResponseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleByAdminResponseDto>>> Handle(ByAdminArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetArticleByAdminAsync(request.PaginationRequest, request.SearchBaseModel, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}