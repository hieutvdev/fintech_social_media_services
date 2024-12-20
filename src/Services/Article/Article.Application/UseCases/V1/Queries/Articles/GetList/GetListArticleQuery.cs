using Article.Application.DTOs.Response.Article;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.Articles.GetList;

public record GetListArticleQuery(PaginationRequest PaginationRequest, SearchBaseModel SearchBaseModel) : IQuery<PaginatedResult<ArticleResponseBaseDto>>;