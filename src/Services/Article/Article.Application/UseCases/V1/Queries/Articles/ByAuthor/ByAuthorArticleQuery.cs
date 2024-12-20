using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.ByAuthor;

public record ByAuthorArticleQuery(PaginationRequest PaginationRequest, SearchBaseModel SearchBaseModel) :IQuery<PaginatedResult<ArticleByUserResponseDto>>;