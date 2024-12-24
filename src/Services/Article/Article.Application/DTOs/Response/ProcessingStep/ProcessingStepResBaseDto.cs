namespace Article.Application.DTOs.Response.ProcessingStep;

public record ProcessingStepResBaseDto(string Id, string Description, int Status, string ArticleReqId, string HandlerId);