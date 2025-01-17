using User.Application.DTOs.Request.Block;

namespace User.Application.UseCases.Commands.Block.Delete;

public record DeleteBlockCommand(DeleteBlockReqDto Payload) : ICommand<bool>;