using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class ArticleRequestPublishId
{
    public Guid Value { get; }
    
    public ArticleRequestPublishId(Guid value) => Value = value;

    public static ArticleRequestPublishId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("ArticleRequestPublishId cannot be empty.");
        }
        
        return new ArticleRequestPublishId(value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is ArticleRequestPublishId articleRequestPublishId)
        {
            return Value.Equals(articleRequestPublishId.Value);
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