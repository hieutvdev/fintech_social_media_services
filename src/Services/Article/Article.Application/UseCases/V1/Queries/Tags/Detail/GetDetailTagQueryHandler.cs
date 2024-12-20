using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.Detail;

public class GetDetailTagQueryHandler(ITagService service) : IQueryHandler<GetDetailTagQuery, Tag>
{
    public async Task<ResultT<Tag>> Handle(GetDetailTagQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetDetailsAsync(request.Id, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}