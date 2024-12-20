using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Categories.Update;

public class UpdateCategoryCommandHandler
(ICategoryService categoryService)
: ICommandHandler<UpdateCategoryCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.UpdateAsync(request.UpdateCategoryRequestDto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}