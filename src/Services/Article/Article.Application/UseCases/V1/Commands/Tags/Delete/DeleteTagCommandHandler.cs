using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Tags.Delete;

public class DeleteTagCommandHandler(ITagService service) : ICommandHandler<DeleteTagCommand, bool>
{
    public async Task<ResultT<bool>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(request.Dto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}