namespace ShredKernel.Aggregates;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string DeletedBy { get; set; }
}