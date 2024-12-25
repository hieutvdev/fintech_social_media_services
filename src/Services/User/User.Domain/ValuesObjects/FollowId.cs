using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class FollowId
{
    public Guid Value { get; }
    
    public FollowId (Guid value) => Value = value;
    
    public static FollowId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("FollowId cannot be empty");
        }
        return new FollowId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is FollowId obj1)
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