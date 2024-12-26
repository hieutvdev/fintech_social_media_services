using ShredKernel.Aggregates;
using User.Domain.Enums;
using User.Domain.ValuesObjects;

namespace User.Domain.Models;

public class FriendRequest : Entity<FriendRequestId>, IAggregateRoot
{
    public string SenderId { get; private set; } = default!;
    
    public string ReceiverId { get; private set; } = default!;
    
    public int Status { get; private set; }
    
    public DateTime? SendAt { get; private set; }
    
    public DateTime? ResponseAt { get; private set; }
    
    
    public static FriendRequest Create(
        FriendRequestId id,
        string senderId,
        string receiverId,
        DateTime? sendAt,
        DateTime? responseAt)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id), "FriendRequestId cannot be null.");

        if (string.IsNullOrWhiteSpace(senderId))
            throw new ArgumentException("SenderId cannot be null or empty.", nameof(senderId));

        if (string.IsNullOrWhiteSpace(receiverId))
            throw new ArgumentException("ReceiverId cannot be null or empty.", nameof(receiverId));

        
        return new FriendRequest
        {
            Id = id,
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = (int)FriendRequestStatus.PEDDING,
            SendAt = sendAt,
            ResponseAt = responseAt
        };
    }
    
    public void Accept(DateTime responseAt)
    {
        Status = (int)FriendRequestStatus.ACCEPTED;
        ResponseAt = responseAt;
    }
    
    public void Reject(DateTime responseAt)
    {
        Status = (int)FriendRequestStatus.REJECTED;
        ResponseAt = responseAt;
    }
    
   
    
}