
using Article.Application.Services;
using Article.Domain.Models;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Categories.GetAll;

public class GetAllCategoryQueryHandler
(ICategoryService categoryService)
: IQueryHandler<GetAllCategoryQuery, IEnumerable<Category>>
{
    public async Task<ResultT<IEnumerable<Category>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetAllAsync(cancellationToken);
        var response =
            Result.Create(value: result);
        return response;
    }
}