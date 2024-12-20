namespace ShredKernel.Aggregates;

public class SoftDelete : ISoftDelete
{
    public bool IsDeleted { get; set; }
    public string DeletedBy { get; set; }
}

