using BuildingBlocks.CQRS.Commands;
using User.Application.DTOs.Request.FriendRequest;

namespace User.Application.UseCases.Commands.FriendRequest.Delete;

public record DeleteFriendRequestCommand(DeleteFriendRequestReqDto Payload) : ICommand<bool>;