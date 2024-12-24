using Article.Application.DTOs.Request.ArticleRequestPublish;
using Article.Application.DTOs.Response.ArticleRequestPublish;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.GetByUser;

public record GetByUserArticleReqPubQuery(ArticleReqSearchListDto Query) : IQuery<PaginatedResult<ArticleRequestPublishResBaseDto>>;