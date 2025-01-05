namespace User.Application.DTOs.Response.FriendRequest;

public record FriendRequestResBase(string Id, string ReceiverId, DateTime? SendAt, string? FullName, string? Avatar);