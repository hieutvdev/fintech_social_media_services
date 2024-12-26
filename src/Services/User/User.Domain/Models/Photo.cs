using ShredKernel.Aggregates;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class Photo : Entity<PhotoId>, IAggregateRoot
{
    public string UserId { get; private set; } = default!;
    
    public string Url { get; private set; } = default!;
    
    public int Privacy { get; private set; }
    
    
    public static Photo Create(PhotoId id, string userId, string url, int privacy)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        
        return new Photo
        {
            Id = id,
            UserId = userId,
            Url = url,
            Privacy = privacy
        };
    }
    
}