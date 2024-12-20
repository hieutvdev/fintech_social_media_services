using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.ByAdmin;

public record ByAdminArticleQuery(PaginationRequest PaginationRequest, SearchBaseModel SearchBaseModel) : IQuery<PaginatedResult<ArticleByAdminResponseDto>>;