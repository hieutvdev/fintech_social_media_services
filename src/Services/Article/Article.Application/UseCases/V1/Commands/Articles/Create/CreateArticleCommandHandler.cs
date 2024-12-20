using Article.Application.Services;
using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace Article.Application.UseCases.V1.Commands.Articles.Create;

public class CreateArticleCommandHandler
(IArticleService repository)
: ICommandHandler<CreateArticleCommand, bool>
{
    public async Task<ResultT<bool>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.CreateAsync(request.CreateArticleRequestDto, cancellationToken);
        var response = Result.Create(result);
        return response;
    }
}