using BuildingBlocks.Exceptions;

namespace Article.Application.Exceptions;

public class ArticleNotFoundException : NotFoundException
{
    public ArticleNotFoundException(string message) : base(message)
    {
        
    }
}