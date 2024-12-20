using Article.Domain.Models;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Categories.Detail;

public record GetDetailCategoryQuery(string Id) : IQuery<Category>;