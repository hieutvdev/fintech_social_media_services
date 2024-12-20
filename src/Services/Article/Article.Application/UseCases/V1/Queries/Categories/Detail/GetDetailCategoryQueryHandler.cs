using Article.Application.Services;
using Article.Domain.Models;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Exceptions.ErrorCodeResponse;
using BuildingBlocks.Helpers;

namespace Article.Application.UseCases.V1.Queries.Categories.Detail;

public class GetDetailCategoryQueryHandler
(ICategoryService categoryService)
: IQueryHandler<GetDetailCategoryQuery, Category>

{
    public async Task<ResultT<Category>> Handle(GetDetailCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetDetailsAsync(request.Id, cancellationToken);
        var response =
            Result.Create(value: result);
            
        return response;
    }
}