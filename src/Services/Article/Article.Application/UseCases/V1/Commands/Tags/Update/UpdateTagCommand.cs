using Article.Application.DTOs.Request.Tag;
using BuildingBlocks.CQRS.Commands;

namespace Article.Application.UseCases.V1.Commands.Tags.Update;

public record UpdateTagCommand(UpdateTagRequestDto Dto) : ICommand<bool>;