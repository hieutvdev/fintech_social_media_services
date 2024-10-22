using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Categories.Delete;

public class DeleteCategoryCommandHandler
(ICategoryService categoryService)
: ICommandHandler<DeleteCategoryCommand, bool>
{
    public Task<ResultT<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}