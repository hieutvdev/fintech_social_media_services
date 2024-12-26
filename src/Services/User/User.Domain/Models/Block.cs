using ShredKernel.Aggregates;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class Block : Entity<BlockId>, IAggregateRoot
{
    public string BlockerId { get; private set; } = default!;

    public string BlockedId { get; private set; } = default!;
    
    
    public static Block Create(
        BlockId id,
        string blockerId,
        string blockedId)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "BlockId cannot be null.");

        if (string.IsNullOrWhiteSpace(blockerId))
            throw new ArgumentException("BlockerId cannot be null or empty.", nameof(blockerId));

        if (string.IsNullOrWhiteSpace(blockedId))
            throw new ArgumentException("BlockedId cannot be null or empty.", nameof(blockedId));

        return new Block
        {
            Id = id,
            BlockerId = blockerId,
            BlockedId = blockedId
        };
    }
    
    
    public void UpdateBlockerId(string blockerId)
    {
        if (string.IsNullOrWhiteSpace(blockerId))
            throw new ArgumentException("BlockerId cannot be null or empty.", nameof(blockerId));

        BlockerId = blockerId;
    }
}