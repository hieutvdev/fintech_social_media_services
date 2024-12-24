namespace Article.Application.DTOs.Request.ProcessingStep;

public class ProcessingStepSearchListDto : SearchListModel
{
    public string? AritcleReqPubId { get; set; }
    
    public string? HandlerId { get; set; }
}