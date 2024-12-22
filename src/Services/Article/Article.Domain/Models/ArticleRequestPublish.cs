using Article.Domain.Enums;
using Article.Domain.ValueObjects;
using ShredKernel.Aggregates;

namespace Article.Domain.Models;

public class ArticleRequestPublish : Entity<ArticleRequestPublishId>
{
    public ArticleId ArticleId { get; private set; } = default!;

    public string Name { get; private set; } = default!;
    
    public string Description { get; private set; } = default!;
    
    public int Status { get; private set; }
    
    private readonly List<ProcessingStep> _processingSteps = new();
    public IReadOnlyList<ProcessingStep> ProcessingSteps => _processingSteps.AsReadOnly();
    
    
    public static ArticleRequestPublish Create(ArticleRequestPublishId articleRequestPublishId, ArticleId articleId, string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var articleRequestPublish = new ArticleRequestPublish
        {
            Id = articleRequestPublishId,
            ArticleId = articleId,
            Name = name,
            Description = description,
            Status = (int)ArticleRequestPublishStatus.PEDDING
        };

        return articleRequestPublish;
    }
    
    public void Update(ArticleId articleId,string name, string description, int status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        if (!Enum.IsDefined(typeof(ArticleRequestPublishStatus), status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status value.");
        }
        ArticleId = articleId;
        Name = name;
        Description = description;
        Status = status;
    }
}