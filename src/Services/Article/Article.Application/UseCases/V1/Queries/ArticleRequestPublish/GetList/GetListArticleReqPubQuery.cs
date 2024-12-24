using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.DTOs.Response.ArticleRequestPublish;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetList;

public record GetListArticleReqPubQuery(ArticleReqSearchListDto Search) : IQuery<PaginatedResult<ArticleRequestPublishResBaseDto>>;