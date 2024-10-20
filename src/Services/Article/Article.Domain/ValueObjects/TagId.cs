using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class TagId
{
    public Guid Value { get; }
    private TagId(Guid value) => Value = value;


    public static TagId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("TagId cannot be empty");
        }

        return new TagId(value);
    }
}