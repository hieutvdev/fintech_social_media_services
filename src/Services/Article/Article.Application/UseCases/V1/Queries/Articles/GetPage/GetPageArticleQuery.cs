using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetPage;

public record GetPageArticleQuery(PaginationRequest Request) : IQuery<PaginatedResult<ArticleResponseBaseDto>>;