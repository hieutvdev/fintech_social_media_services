using Article.Application.DTOs.Response.ProcessingStep;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.GetAll;

public class GetAllProcessingStepQueryHandler
(IProcessingStepService service)
: IQueryHandler<GetAllProcessingStepQuery, IEnumerable<ProcessingStepResBaseDto>>
{
    public async Task<ResultT<IEnumerable<ProcessingStepResBaseDto>>> Handle(GetAllProcessingStepQuery request, CancellationToken cancellationToken)
    {
        var result = await service.GetAllAsync(cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}