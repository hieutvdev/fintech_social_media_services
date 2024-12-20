using Article.Application.Services;
using Article.Domain.Models;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;

namespace Article.Application.UseCases.V1.Queries.Categories.GetPage;

public class GetPageCategoryQueryHandler
(ICategoryService categoryService)
: IQueryHandler<GetPageCategoryQuery, PaginatedResult<Category>>
{
    public async Task<ResultT<PaginatedResult<Category>>> Handle(GetPageCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryService.GetPageAsync(request.PaginationRequest, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
    
}