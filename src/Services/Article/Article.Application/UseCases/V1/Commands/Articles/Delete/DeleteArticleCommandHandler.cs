using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Articles.Delete;

public class DeleteArticleCommandHandler(IArticleService service) : ICommandHandler<DeleteArticleCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.DeleteArticleRequestDto, cancellationToken);
        return Result.Create(result);
    }
}