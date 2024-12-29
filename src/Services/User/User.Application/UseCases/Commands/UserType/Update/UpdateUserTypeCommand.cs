using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.UserType;

namespace User.Application.UseCases.Commands.UserType.Update;

public record UpdateUserTypeCommand(UpdateUserTypeReqDto Payload) : ICommand<bool>;