namespace Article.Application.DTOs.Request.ProcessingStep;

public record DeleteProcessingRequestDto(IEnumerable<string> Ids);