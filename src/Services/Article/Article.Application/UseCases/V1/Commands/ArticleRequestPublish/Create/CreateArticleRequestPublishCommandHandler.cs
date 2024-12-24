using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.ArticleRequestPublish.Create;

public class CreateArticleRequestPublishCommandHandler
(IArticleRequestPublishService service) 
: ICommandHandler<CreateArticleRequestPublishCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateArticleRequestPublishCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Payload, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}