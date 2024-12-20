using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Tags.Create;

public class CreateTagCommandHandler(ITagService service) : ICommandHandler<CreateTagCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request.Dto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}