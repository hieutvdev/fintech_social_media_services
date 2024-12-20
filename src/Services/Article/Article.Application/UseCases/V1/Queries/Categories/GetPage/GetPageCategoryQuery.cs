using Article.Domain.Models;
using BuildingBlocks.CQRS.Queries;
using BuildingBlocks.Pagination;

namespace Article.Application.UseCases.V1.Queries.Categories.GetPage;

public record GetPageCategoryQuery(PaginationRequest PaginationRequest) : IQuery<PaginatedResult<Category>>;