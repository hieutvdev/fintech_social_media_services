using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class FriendShipId
{
    public Guid Value { get; }
    
    public FriendShipId (Guid value) => Value = value;
    
    public static FriendShipId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("FriendShipId cannot be empty");
        }
        return new FriendShipId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is FriendShipId obj1)
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