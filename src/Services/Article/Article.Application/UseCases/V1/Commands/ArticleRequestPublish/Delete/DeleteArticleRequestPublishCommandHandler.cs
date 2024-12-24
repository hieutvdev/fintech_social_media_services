using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Delete;

public class DeleteArticleRequestPublishCommandHandler
(IArticleRequestPublishService service) : ICommandHandler<DeleteArticleRequestPublishCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteArticleRequestPublishCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}