using Article.Application.DTOs.Response.ArticleRequestPublish;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ArticleRequestPublish.Detail;

public record GetDetailArticleReqPubQuery(string Id) : IQuery<ArticleReqPublishDetailResDto>;