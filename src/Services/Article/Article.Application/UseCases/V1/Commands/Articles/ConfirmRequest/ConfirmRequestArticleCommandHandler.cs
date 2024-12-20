using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Articles.ConfirmRequest;

public class ConfirmRequestArticleCommandHandler(IArticleService service) : ICommandHandler<ConfirmRequestArticleCommand, bool>
{
    public async Task<ResultT<bool>> Handle(ConfirmRequestArticleCommand request, CancellationToken cancellationToken)
    {
        var result = await service.ConfirmRequestArticleByAdminAsync(request.Request, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}