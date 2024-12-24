using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.GetByUser;

public class GetByUserProcessingStepQueryHandler
(IProcessingStepService service)
: IQueryHandler<GetByUserProcessingStepQuery, PaginatedResult<ProcessingStepResBaseDto>>
{
    public async Task<ResultT<PaginatedResult<ProcessingStepResBaseDto>>> Handle(GetByUserProcessingStepQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetByUserAsync(request.Search, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}