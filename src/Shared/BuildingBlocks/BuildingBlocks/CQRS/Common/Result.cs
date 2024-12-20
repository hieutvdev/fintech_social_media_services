namespace BuildingBlocks.CQRS.Common;

public class Result
{
    protected internal Result(bool isSuccess, Error error, string message = "")
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
        Message = message;
    }
    
    
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    public string Message { get; }
    public Error Error { get; }

    public static Result Success(string message) => new Result(true, Error.None,  message);
    public static ResultT<TValue> Success<TValue>(TValue value, string message) => new(value, true, Error.None, message);

    public static ResultT<TValue> Success<TValue>(string message) => new(default, true, Common.Error.None, message);
    public static Result Failure(Error error) => new Result(false, error, "");
    public static ResultT<TValue> Failure<TValue>(Error error, string message) => new(default, false, error, "");
    
    public static ResultT<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value, "Success") : Failure<TValue>(Error.NullValue, "Failure");


}