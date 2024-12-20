using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetSelect;

public class GetSelectArticleQueryHandler(IArticleService service) : IQueryHandler<GetSelectArticleQuery, IEnumerable<SelectListItem>>
{
    public async Task<ResultT<IEnumerable<SelectListItem>>> Handle(GetSelectArticleQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetSelectAsync(cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}