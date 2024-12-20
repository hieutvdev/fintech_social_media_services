using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.RelateByCategoryName;

public record RelateByCategoryNameQuery(string CategoryNames) : IQuery<IEnumerable<ArticleRelateResponseDto>>;