namespace BuildingBlocks.CQRS.Common;

public class ResultT<TValue> : Result
{
    private readonly TValue? _value;

    protected internal ResultT(TValue? value, bool isSuccess, Error error, string message) : base(isSuccess, error, message) => _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed");
    

    public static implicit operator ResultT<TValue>(TValue value) => Create(value);
}