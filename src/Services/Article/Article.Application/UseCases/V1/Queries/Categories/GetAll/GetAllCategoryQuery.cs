using Article.Domain.Models;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Categories.GetAll;

public record GetAllCategoryQuery () : IQuery<IEnumerable<Category>>;