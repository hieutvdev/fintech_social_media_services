using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetAll;

public class GetAllArticleQueryHandler
(IArticleService articleService)
: IQueryHandler<GetAllArticleQuery, IEnumerable<Domain.Models.Article>>
{
    public async Task<ResultT<IEnumerable<Domain.Models.Article>>> Handle(GetAllArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await articleService.GetAllAsync(cancellationToken);
        var response = Result.Create(result);

        return response;
    }
}