using Article.Application.DTOs.Response.Category;
using Article.Application.Services;
using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;

namespace Article.Application.UseCases.V1.Queries.Categories.GetList;

public class GetListCategoryQueryHandler
(ICategoryService categoryService)
:  IQueryHandler<GetListCategoryQuery, PaginatedResult<CategoryResponseBaseDto>>
{
    public async Task<ResultT<PaginatedResult<CategoryResponseBaseDto>>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        var result =
            await categoryService.GetListAsync(request.PaginationRequest, request.SearchBaseModel, cancellationToken);
        var response = Result.Create(result);

        return response;
    }
    
}