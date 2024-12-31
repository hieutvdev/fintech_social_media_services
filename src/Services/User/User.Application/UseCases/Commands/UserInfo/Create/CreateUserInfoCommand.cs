using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.UserInfo;

namespace User.Application.UseCases.Commands.UserInfo.Create;

public record CreateUserInfoCommand(CreateUserInfoReqDto Payload) : ICommand<bool>;