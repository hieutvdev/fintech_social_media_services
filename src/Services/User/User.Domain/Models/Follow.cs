using ShredKernel.Aggregates;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class Follow : Entity<FollowId>, IAggregateRoot
{
    public string FollowerId { get; private set; } = default!;
    
    public string FollowingId { get; private set; } = default!;
    
    
    public static Follow Create(
        FollowId id,
        string followerId,
        string followingId)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "FollowId cannot be null.");

        if (string.IsNullOrWhiteSpace(followerId))
            throw new ArgumentException("FollowerId cannot be null or empty.", nameof(followerId));

        if (string.IsNullOrWhiteSpace(followingId))
            throw new ArgumentException("FollowingId cannot be null or empty.", nameof(followingId));

        return new Follow
        {
            Id = id,
            FollowerId = followerId,
            FollowingId = followingId
        };
    }
    
    public void Update(string followerId, string followingId)
    {
        if (string.IsNullOrWhiteSpace(followerId))
            throw new ArgumentException("FollowerId cannot be null or empty.", nameof(followerId));

        if (string.IsNullOrWhiteSpace(followingId))
            throw new ArgumentException("FollowingId cannot be null or empty.", nameof(followingId));

        FollowerId = followerId;
        FollowingId = followingId;
    }
    
}