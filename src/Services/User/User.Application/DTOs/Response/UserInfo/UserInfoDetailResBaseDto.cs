namespace User.Application.DTOs.Response.UserInfo;

public record UserInfoDetailResBaseDto(
        string Id,
        string UserId,
        string CoverPhoto,
        int Gender,
        string Bio,
        string CurrentCity,
        string Education,
        IEnumerable<string> Work,
        IEnumerable<string> Skills,
        IEnumerable<string> Hobbies,
        string UserType,
        int RelationshipStatus
    );