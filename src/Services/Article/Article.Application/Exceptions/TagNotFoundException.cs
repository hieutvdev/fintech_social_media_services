using BuildingBlocks.Exceptions;

namespace Article.Application.Exceptions;

public class TagNotFoundException: NotFoundException
{
    public TagNotFoundException(string message) : base(message)
    {
        
    }
}