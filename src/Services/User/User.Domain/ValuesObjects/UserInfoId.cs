using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class UserInfoId
{
    public Guid Value { get; }
    
    public UserInfoId (Guid value) => Value = value;
    
    public static UserInfoId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("UserInfoId cannot be empty");
        }
        return new UserInfoId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is UserInfoId obj1)
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