using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Delete;

public class DeleteProcessingStepCommandHandler
(IProcessingStepService service)
: ICommandHandler<DeleteProcessingStepCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteProcessingStepCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}