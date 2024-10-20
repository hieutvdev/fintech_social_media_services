using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class AuthorId
{
    public Guid Value { get; }

    private AuthorId(Guid value) => Value = value;

    public static AuthorId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("AuthorId cannot be empty");
        }

        return new AuthorId(value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is AuthorId authorId)
        {
            return Value.Equals(authorId.Value);
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