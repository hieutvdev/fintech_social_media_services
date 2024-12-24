namespace Article.Application.DTOs.Request.ProcessingStep;

public record UpdateProcessingRequestDto(string Id, string Description, int Status, string HandlerId);