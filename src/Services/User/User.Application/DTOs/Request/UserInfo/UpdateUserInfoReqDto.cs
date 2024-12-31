namespace User.Application.DTOs.Request.UserInfo;

public record UpdateUserInfoReqDto(
    string Id,
    string UserId,
    string? CoverPhoto,
    int Gender,
    string? Bio,
    string? CurrentCity,
    string? Education,
    string? Work,
    string Skills,
    int RelationshipStatus,
    string? Hobbies
    );