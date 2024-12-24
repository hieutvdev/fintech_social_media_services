using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Update;

public class UpdateArticleRequestPublishCommandHandler
(IArticleRequestPublishService service) : ICommandHandler<UpdateArticleRequestPublishCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateArticleRequestPublishCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
        
    }
}