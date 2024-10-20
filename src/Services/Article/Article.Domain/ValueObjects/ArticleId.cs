using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class ArticleId
{
    public Guid Value { get; }
    private ArticleId(Guid value) => Value = value;

    public static ArticleId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("Article cannot be empty");
        }

        return new ArticleId(value);
    }
}