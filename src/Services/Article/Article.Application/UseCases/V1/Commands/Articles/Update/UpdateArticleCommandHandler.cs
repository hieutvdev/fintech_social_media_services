using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Articles.Update;

public class UpdateArticleCommandHandler(IArticleService service) : ICommandHandler<UpdateArticleCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request.Request, cancellationToken);
        return Result.Create(result);
    }
}