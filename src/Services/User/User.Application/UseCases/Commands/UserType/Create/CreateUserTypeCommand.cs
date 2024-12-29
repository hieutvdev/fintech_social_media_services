using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.UserType;

namespace User.Application.UseCases.Commands.UserType.Create;

public record CreateUserTypeCommand(CreateUserTypeReqDto Payload) : ICommand<bool>;