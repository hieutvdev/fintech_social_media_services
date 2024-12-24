using BuildingBlocks.Exceptions;

namespace Article.Application.Exceptions;

public class ArticleReqPubNotFoundException : NotFoundException
{
    public ArticleReqPubNotFoundException(string message) : base(message)
    {
        
    }
}