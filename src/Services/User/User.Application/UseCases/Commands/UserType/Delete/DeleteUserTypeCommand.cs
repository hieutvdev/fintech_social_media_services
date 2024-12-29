using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.UserType;

namespace User.Application.UseCases.Commands.UserType.Delete;

public record DeleteUserTypeCommand(DeleteUserTypeReqDto Payload) : ICommand<bool>;