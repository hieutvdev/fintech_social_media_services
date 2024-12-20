using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.ByAuthor;

public class ByAuthorArticleQueryHandler(IArticleService service) : IQueryHandler<ByAuthorArticleQuery, PaginatedResult<ArticleByUserResponseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleByUserResponseDto>>> Handle(ByAuthorArticleQuery request, CancellationToken cancellationToken)
    {
        var result =
            await service.GetArticleByUserAsync(request.PaginationRequest, request.SearchBaseModel, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}