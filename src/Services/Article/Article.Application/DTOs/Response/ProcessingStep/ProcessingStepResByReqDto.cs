namespace Article.Application.DTOs.Response.ProcessingStep;

public record ProcessingStepResByReqDto(string Id, string Description, string HandlerName, int Status, string HandlerId);