using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.Detail;

public class GetDetailProcessingStepQueryHandler
(IProcessingStepService service)
: IQueryHandler<GetDetailProcessingStepQuery, ProcessingStepResBaseDto>
{
    public async Task<ResultT<ProcessingStepResBaseDto>> Handle(GetDetailProcessingStepQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(request.Id, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}