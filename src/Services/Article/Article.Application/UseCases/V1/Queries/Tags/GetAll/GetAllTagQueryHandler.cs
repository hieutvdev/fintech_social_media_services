using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetAll;

public class GetAllTagQueryHandler(ITagService service) : IQueryHandler<GetAllTagQuery, IEnumerable<Tag>>
{
    public async Task<ResultT<IEnumerable<Tag>>> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(cancellationToken);
        var response = Result.Create(result);
        return response;

    }
}