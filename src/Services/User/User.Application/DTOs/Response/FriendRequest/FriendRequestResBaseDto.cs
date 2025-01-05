namespace User.Application.DTOs.Response.FriendRequest;

public record FriendRequestResBaseDto(string SenderId, string ReceiverId, int Status, DateTime? SendAt);