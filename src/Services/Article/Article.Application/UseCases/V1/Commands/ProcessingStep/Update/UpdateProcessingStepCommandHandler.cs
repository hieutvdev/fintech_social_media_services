using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Update;

public class UpdateProcessingStepCommandHandler
(IProcessingStepService service)
: ICommandHandler<UpdateProcessingStepCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateProcessingStepCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}