using Article.Application.DTOs.Response.Article;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.RelateByCategoryName;

public class RelateByCategoryNameQueryHandler(IArticleService service) : IQueryHandler<RelateByCategoryNameQuery, IEnumerable<ArticleRelateResponseDto>>
{
    public async Task<ResultT<IEnumerable<ArticleRelateResponseDto>>> Handle(RelateByCategoryNameQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetArticleRelateByCateNameAsync(request.CategoryNames, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}