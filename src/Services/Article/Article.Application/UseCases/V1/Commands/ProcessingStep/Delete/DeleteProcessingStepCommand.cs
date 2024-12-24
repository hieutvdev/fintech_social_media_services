using Article.Application.DTOs.Request.ProcessingStep;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Delete;

public record DeleteProcessingStepCommand(DeleteProcessingRequestDto Payload) : ICommand<bool>;