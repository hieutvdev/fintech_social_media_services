namespace BuildingBlocks.CQRS.Common;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    
    
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new Result(true, Error.None);
    public static ResultT<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static ResultT<TValue> Failure<TValue>(Error error) => new(default, false, error);
    
    public static ResultT<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);


}