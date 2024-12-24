namespace Article.Application.DTOs.Request.ArticleRequestPublish;

public class ArticleReqSearchListDto : SearchListModel
{
    public string? ArticleId { get; set; }
    
    public string? HandlerId { get; set; }
    
}