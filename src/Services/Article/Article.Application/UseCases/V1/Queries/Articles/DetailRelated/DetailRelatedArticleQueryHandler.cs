using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.DetailRelated;

public class DetailRelatedArticleQueryHandler(IArticleService service) : IQueryHandler<DetailRelatedArticleQuery, ArticleDetailRelateResponseDto>
{
    public async Task<ResultT<ArticleDetailRelateResponseDto>> Handle(DetailRelatedArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetRelateDetails(request.Id, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}