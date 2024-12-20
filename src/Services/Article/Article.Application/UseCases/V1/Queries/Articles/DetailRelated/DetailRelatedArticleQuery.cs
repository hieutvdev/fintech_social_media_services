using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.DetailRelated;

public record DetailRelatedArticleQuery(string Id) : IQuery<ArticleDetailRelateResponseDto>;