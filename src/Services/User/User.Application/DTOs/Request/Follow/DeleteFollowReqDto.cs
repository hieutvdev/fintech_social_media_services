namespace User.Application.DTOs.Request.Follow;

public record DeleteFollowReqDto(IEnumerable<string> UserIds);
