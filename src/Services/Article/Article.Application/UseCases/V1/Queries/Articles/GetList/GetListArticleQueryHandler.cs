using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetList;

public class GetListArticleQueryHandler(IArticleService service) : IQueryHandler<GetListArticleQuery, PaginatedResult<ArticleResponseBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ArticleResponseBaseDto>>> Handle(GetListArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(request.PaginationRequest, request.SearchBaseModel, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}