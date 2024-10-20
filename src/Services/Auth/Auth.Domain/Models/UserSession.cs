using Auth.Domain.Enums;
using Auth.Domain.ValueObjects;
using ShredKernel.Aggregates;

namespace Auth.Domain.Models;


#nullable disable
public class UserSession : Entity<UserSessionId>
{
    public string UserId { get; private set; } = default!;
    public string DeviceId { get; private set; } = default!;
    public DeviceType DeviceType { get; private set; }
    public string PushToken { get; private set; } = default!;
    public virtual ApplicationUser User { get; set; }

    public static UserSession Create(string userId, string deviceId, DeviceType deviceType, string pushToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentException.ThrowIfNullOrEmpty(deviceId);

        var userSession = new UserSession
        {
            Id = UserSessionId.Of(Guid.NewGuid()),
            UserId = userId,
            DeviceId = deviceId,
            DeviceType = deviceType,
            PushToken = pushToken
        };
        return userSession;
    }
}