using Article.Application.DTOs.Request.ProcessingStep;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.ProcessingStep.Update;

public record UpdateProcessingStepCommand(UpdateProcessingRequestDto Payload) : ICommand<bool>;