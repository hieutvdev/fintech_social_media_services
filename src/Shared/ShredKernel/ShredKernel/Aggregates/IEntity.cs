namespace ShredKernel.Aggregates;

public interface IEntity<T> : IEntity
{
    public T Id
    {
        get; set;
    }
}

public interface IEntity : IDateTracking
{

}