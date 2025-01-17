using User.Application.DTOs.Request.Block;

namespace User.Application.UseCases.Commands.Block.Create;

public record CreateBlockCommand(CreateBlockReqDto Payload) : ICommand<bool>;
