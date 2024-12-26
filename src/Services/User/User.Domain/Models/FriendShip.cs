using ShredKernel.Aggregates;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class FriendShip : Entity<FriendRequestId> , IAggregateRoot
{
    public string UserId { get; private set; } = default!;
    
    public string FriendId { get; private set; } = default!;
    
    public int Status { get; private set; }


    public static FriendShip Create(FriendRequestId id, string userId, string friendId)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));
        ArgumentException.ThrowIfNullOrWhiteSpace(friendId, nameof(friendId));
        
        return new FriendShip
        {
            Id = id,
            UserId = userId,
            FriendId = friendId,
            Status = 0
        };
    }
    
    
}