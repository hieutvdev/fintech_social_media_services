namespace User.Application.DTOs.Response.FriendRequest;

public record FriendRequestByUserResDto(string Id, string ReceiverId, DateTime? SendAt);