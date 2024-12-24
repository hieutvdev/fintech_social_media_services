namespace Article.Application.DTOs.Request.ProcessingStep;

public record CreateProcessingRequestDto(string ArticleRequestPublishId, string Description, string HandlerId, int Status);