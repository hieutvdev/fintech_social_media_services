
using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.Helpers;
using HttpStatusCode = BuildingBlocks.Exceptions.ErrorCodeResponse.HttpStatusCode;

namespace Article.Application.UseCases.V1.Commands.Categories.Create;

public class CreateCategoryCommandHandler(ICategoryService categoryService)
: ICommandHandler<CreateCategoryCommand>
{
    public async Task<Result> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await categoryService.CreateAsync(request.CreateCategoryRequestDto, cancellationToken);
        var response = result ? Result.Success() : Result.Failure(CodeResponseHelper.GetErrorByCode(HttpStatusCode.ErrBadRequest));
        return response;
    }
}