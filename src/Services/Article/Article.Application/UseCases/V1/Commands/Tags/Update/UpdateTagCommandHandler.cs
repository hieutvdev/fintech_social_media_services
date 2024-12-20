using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Tags.Update;

public class UpdateTagCommandHandler(ITagService service) : ICommandHandler<UpdateTagCommand, bool>
{
    public async Task<ResultT<bool>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(request.Dto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}