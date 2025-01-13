namespace User.Application.DTOs.Request.UserInfo;

public record CreateUserInfoReqDto(
    string UserId,
    int Gender,
    string FullName,
    string AvatarUrl,
    int BirthDay
    );