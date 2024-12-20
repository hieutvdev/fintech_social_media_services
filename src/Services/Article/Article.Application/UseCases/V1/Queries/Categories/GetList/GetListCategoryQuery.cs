using Article.Application.DTOs.Response.Category;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;

namespace Article.Application.UseCases.V1.Queries.Categories.GetList;

public record GetListCategoryQuery(PaginationRequest PaginationRequest, SearchBaseModel SearchBaseModel) : IQuery<PaginatedResult<CategoryResponseBaseDto>>;