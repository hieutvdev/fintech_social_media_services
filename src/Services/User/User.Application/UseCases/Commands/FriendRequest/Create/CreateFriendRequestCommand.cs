using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.FriendRequest;

namespace User.Application.UseCases.Commands.FriendRequest.Create;

public record CreateFriendRequestCommand(CreateFriendRequestReqDto Payload) : ICommand<bool>;