using MassTransit.Middleware;
using ShredKernel.Aggregates;
using User.Domain.Enums;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class UserInfo : Entity<UserInfoId>, IAggregateRoot
{
    public string UserId { get; private set; } = default!;

    public string? CoverPhoto { get; private set; } = default!;

    public int Gender { get; private set; }

    public string? Bio { get; private set; }

    public string? CurrentCity { get; private set; }

    public string? Education { get; private set; }

    public string? Work { get; private set; }

    public string? Skills { get; private set; }

    public int RelationshipStatus { get; private set; }

    public int UserType { get; private set; }

    public string? Hobbies { get; private set; }
    


    public static UserInfo Create(
        UserInfoId id,
        string userId,
        string? coverPhoto,
        int gender,
        string? bio,
        string? currentCity,
        string? education,
        string? work,
        string? skills,
        int userType,
        string? hobbies)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "UserInfoId cannot be null.");

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

        if (gender < 0 || gender > 2)
            throw new ArgumentOutOfRangeException(nameof(gender), "Gender must be 0, 1, or 2.");

        if (userType < 0)
            throw new ArgumentOutOfRangeException(nameof(userType), "UserType must be a non-negative value.");

        return new UserInfo
        {
            Id = id,
            UserId = userId,
            CoverPhoto = coverPhoto,
            Gender = gender,
            Bio = bio,
            CurrentCity = currentCity,
            Education = education,
            Work = work,
            Skills = skills,
            RelationshipStatus = (int)UserRelationShipStatus.SINGLE,
            UserType = userType,
            Hobbies = hobbies
        };
    }

    public void Update(
        string? coverPhoto,
        int gender,
        string? bio,
        string? currentCity,
        string? education,
        string? work,
        string? skills,
        int userType,
        string? hobbies,
        int relationshipStatus)
    {
        ArgumentOutOfRangeException.ThrowIfZero(relationshipStatus);

        if (!Enum.IsDefined(typeof(UserRelationShipStatus), relationshipStatus))
        {
            throw new ArgumentOutOfRangeException(nameof(relationshipStatus), "Invalid relationshipStatus value.");
        }

        if (!Enum.IsDefined(typeof(Gender), gender))
        {
            throw new ArgumentOutOfRangeException(nameof(relationshipStatus), "Invalid gender value.");
        }

        CoverPhoto = coverPhoto;
        Gender = gender;
        Bio = bio;
        CurrentCity = currentCity;
        Education = education;
        Work = work;
        Skills = skills;
        UserType = userType;
        Hobbies = hobbies;
        RelationshipStatus = relationshipStatus;
        
    }



}