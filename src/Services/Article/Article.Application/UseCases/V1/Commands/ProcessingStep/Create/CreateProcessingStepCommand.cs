using Article.Application.DTOs.Request.ProcessingStep;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Create;

public record CreateProcessingStepCommand(CreateProcessingRequestDto Payload) : ICommand<bool>;