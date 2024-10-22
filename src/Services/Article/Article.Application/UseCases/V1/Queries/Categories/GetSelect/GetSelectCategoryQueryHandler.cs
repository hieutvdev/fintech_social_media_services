using Article.Application.DTOs.Response.Category;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Categories.GetSelect;

public class GetSelectCategoryQueryHandler
(ICategoryService categoryService)
: IQueryHandler<GetSelectCategoryQuery, IEnumerable<CategoryGetSelectResponseDto>>
{
    public async Task<ResultT<IEnumerable<CategoryGetSelectResponseDto>>> Handle(GetSelectCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetSelectAsync(cancellationToken);
        var response = Result.Create(value: result);
        return response;
    }
}