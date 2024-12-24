using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.GetList;

public class GetListProcessingStepQueryHandler
(IProcessingStepService service)
: IQueryHandler<GetListProcessingStepQuery, PaginatedResult<ProcessingStepResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ProcessingStepResBaseDto>>> Handle(GetListProcessingStepQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(request.Search, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}