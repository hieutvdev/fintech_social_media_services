using BuildingBlocks.Exceptions;

namespace Article.Application.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(string message) : base(message)
    {
        
    }
}