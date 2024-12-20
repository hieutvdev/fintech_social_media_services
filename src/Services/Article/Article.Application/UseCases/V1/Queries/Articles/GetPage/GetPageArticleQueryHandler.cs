using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetPage;

public class GetPageArticleQueryHandler(IArticleService service) : IQueryHandler<GetPageArticleQuery, PaginatedResult<ArticleResponseBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleResponseBaseDto>>> Handle(GetPageArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetPageAsync(request.Request, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}