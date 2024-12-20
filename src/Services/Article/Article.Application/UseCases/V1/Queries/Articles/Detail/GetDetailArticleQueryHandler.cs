using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.Detail;

public class GetDetailArticleQueryHandler(IArticleService service) : IQueryHandler<GetDetailArticleQuery, Article.Domain.Models.Article>
{
    public async Task<ResultT<Article.Domain.Models.Article>> Handle(GetDetailArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetDetailAsync(request.Id, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}