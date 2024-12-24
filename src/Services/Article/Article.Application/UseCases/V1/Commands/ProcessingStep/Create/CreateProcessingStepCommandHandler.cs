using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Create;

public class CreateProcessingStepCommandHandler
(IProcessingStepService service)
: ICommandHandler<CreateProcessingStepCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateProcessingStepCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}