using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class CategoryId
{
    public Guid Value { get; }

    private CategoryId(Guid value) => Value = value;

    public static CategoryId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("CategoryId cannot be empty");
        }

        return new CategoryId(value);
    }
}