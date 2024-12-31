using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.UserInfo;

namespace User.Application.UseCases.Commands.UserInfo.Update;

public record UpdateUserInfoCommand(UpdateUserInfoReqDto Payload) : ICommand<bool>;