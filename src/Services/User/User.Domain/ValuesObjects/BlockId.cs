using User.Domain.Exceptions;

namespace User.Domain.ValuesObjects;

public class BlockId
{
    public Guid Value { get; }
    
    public BlockId (Guid value) => Value = value;
    
    public static BlockId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("BlockId cannot be empty");
        }
        return new BlockId(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is BlockId obj1)
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