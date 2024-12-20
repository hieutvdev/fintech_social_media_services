
using Article.Application.DTOs.Request.Category;
using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.Exceptions.ErrorCodeResponse;
using BuildingBlocks.Helpers;

namespace Article.Application.UseCases.V1.Commands.Categories.Delete;

public class DeleteCategoryCommandHandler
(ICategoryService categoryService)
: ICommandHandler<DeleteCategoryCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.DeleteAsync(new DeleteCategoryRequestDto(request.Ids), cancellationToken);
        var response = result
            ? Result.Success<bool>("Update category successful")
            : Result.Failure<bool>(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest), "Update categoryFailure");
        return response;
    }
}

