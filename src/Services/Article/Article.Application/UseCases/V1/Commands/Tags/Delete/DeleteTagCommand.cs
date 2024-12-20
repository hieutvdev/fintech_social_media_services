using Article.Application.DTOs.Request.Tag;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Tags.Delete;

public record DeleteTagCommand(DeleteTagRequestDto Dto) : ICommand<bool>;