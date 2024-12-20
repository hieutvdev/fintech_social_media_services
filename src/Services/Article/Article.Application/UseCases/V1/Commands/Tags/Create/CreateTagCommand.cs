using Article.Application.DTOs.Request.Tag;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Tags.Create;

public record CreateTagCommand(CreateTagRequestDto Dto) : ICommand<bool>;