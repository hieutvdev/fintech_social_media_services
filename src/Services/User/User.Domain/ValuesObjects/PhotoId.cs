using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class PhotoId
{
    public Guid Value { get; }
    
    public PhotoId (Guid value) => Value = value;
    
    public static PhotoId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("PhotoId cannot be empty");
        }
        return new PhotoId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is PhotoId obj1)
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