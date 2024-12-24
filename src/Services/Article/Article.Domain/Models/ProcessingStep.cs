using Article.Domain.Enums;
using Article.Domain.ValueObjects;
using ShredKernel.Aggregates;

namespace Article.Domain.Models;

public class ProcessingStep : Entity<ProcessingStepId>, IAggregateRoot
{
    public ArticleRequestPublishId ArticleRequestPublishId { get; private set; } = default!;
    
    public string Description { get; private set; } = default!;
    
    public int Status { get; private set; }
    
    public string HandlerId { get; private set; } = default!;
    
    
    public static ProcessingStep Create(ProcessingStepId processingStepId, ArticleRequestPublishId articleRequestPublishId, string description, string handlerId, ProcessingStepStatus status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(handlerId);

        var processingStep = new ProcessingStep
        {
            Id = processingStepId,
            ArticleRequestPublishId = articleRequestPublishId,
            Description = description,
            HandlerId = handlerId,
            Status = (int)status
        };

        return processingStep;
    }
    
    
    
    public void Update(string description, int status, string handlerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(handlerId);

        if (!Enum.IsDefined(typeof(ProcessingStepStatus), status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status value.");
        }
        
        Description = description;
        Status = status;
        HandlerId = handlerId;
    }
    
}