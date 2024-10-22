using Article.Application.DTOs.Response.Category;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Categories.GetSelect;

public record GetSelectCategoryQuery() : IQuery<IEnumerable<CategoryGetSelectResponseDto>>;