namespace Auth.Domain.Enums;

public enum UserStatus
{
    Active = 1,
    NoConfirm = 2,
    Locked = 3,
    SelfLocking = 4,
    Ban = 5,
    Deleted = 6,
}