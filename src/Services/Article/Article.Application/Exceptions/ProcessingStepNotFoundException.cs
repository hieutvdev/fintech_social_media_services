using BuildingBlocks.Exceptions;

namespace Article.Application.Exceptions;

public class ProcessingStepNotFoundException : NotFoundException
{
    public ProcessingStepNotFoundException(string message) : base(message)
    {
    }
}