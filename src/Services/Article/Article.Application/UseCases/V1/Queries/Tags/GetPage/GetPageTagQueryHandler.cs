using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetPage;

public class GetPageTagQueryHandler(ITagService service) : IQueryHandler<GetPageTagQuery,PaginatedResult<Tag>>
{
    public async Task<ResultT<PaginatedResult<Tag>>> Handle(GetPageTagQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetPageAsync(request.PaginationRequest, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}