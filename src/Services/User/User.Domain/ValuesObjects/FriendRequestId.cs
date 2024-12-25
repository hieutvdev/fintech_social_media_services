using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class FriendRequestId
{
    public Guid Value { get;  }
    
    public FriendRequestId(Guid value) => Value = value;

    public static FriendRequestId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("FriendRequestId can not be empty");
        }
        
        return new FriendRequestId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is FriendRequestId obj1)
        {
            return Value.Equals(obj1.Value);
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}