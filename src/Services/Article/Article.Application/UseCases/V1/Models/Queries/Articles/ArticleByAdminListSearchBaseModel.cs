namespace Article.Application.UseCases.V1.Models.Queries.Articles;

public class ArticleByAdminListSearchBaseModel : SearchBaseModel
{
    public string? AuthorId { get; set; }
    
    public ArticleByAdminListSearchBaseModel(string sortBy, bool? isAscending, string? keySearch, DateTime? startDate, DateTime? endDate, string? authorId) : base(sortBy, isAscending, keySearch, startDate, endDate)
    {
        AuthorId = authorId;
    }
}