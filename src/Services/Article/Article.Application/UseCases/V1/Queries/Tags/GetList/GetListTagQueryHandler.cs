using Article.Application.DTOs.Response.Tag;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Tags.GetList;

public class GetListTagQueryHandler(ITagService service) : IQueryHandler<GetListTagQuery, PaginatedResult<TagResponseBaseDto>>
{
    public async Task<ResultT<PaginatedResult<TagResponseBaseDto>>> Handle(GetListTagQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(request.PaginationRequest, request.SearchBaseModel, cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}