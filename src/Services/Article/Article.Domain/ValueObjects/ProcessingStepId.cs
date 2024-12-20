using Article.Domain.Exceptions;

namespace Article.Domain.ValueObjects;

public class ProcessingStepId
{
    public Guid Value { get; }
    
    private ProcessingStepId(Guid value) => Value = value;


    public static ProcessingStepId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
        {
            throw new DomainException("ProcessingStepId cannot be empty.");
        }

        return new ProcessingStepId(value);
    }


    public override bool Equals(object? obj)
    {
        if (obj is ProcessingStepId processingStepId)
        {
            return Value.Equals(processingStepId.Value);
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