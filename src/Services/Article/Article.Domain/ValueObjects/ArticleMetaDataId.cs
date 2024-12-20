using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class ArticleMetaDataId
{
    public Guid Value { get; }
    private ArticleMetaDataId(Guid value) => Value = value;

    public static ArticleMetaDataId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("ArticleMetaDataId cannot be empty");
        }

        return new ArticleMetaDataId(value);
    }
}