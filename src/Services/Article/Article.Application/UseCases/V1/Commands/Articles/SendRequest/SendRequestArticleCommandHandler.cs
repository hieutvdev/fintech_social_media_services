using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Articles.SendRequest;

public class SendRequestArticleCommandHandler(IArticleService service) : ICommandHandler<SendRequestArticleCommand, bool>
{
    public async Task<ResultT<bool>> Handle(SendRequestArticleCommand request, CancellationToken cancellationToken)
    {
        var result = await service.SendRequestArticleByUserAsync(request.Request, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}