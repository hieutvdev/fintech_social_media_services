using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetSelect;

public class GetSelectTagQueryHandler(ITagService service) : IQueryHandler<GetSelectTagQuery, IEnumerable<SelectListItem>>
{
    public async Task<ResultT<IEnumerable<SelectListItem>>> Handle(GetSelectTagQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetSelectAsync(cancellationToken);
        var response = Result.Create(result);
        return response;
    }
} 