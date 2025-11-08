namespace NewerDown.Domain.Result;

public class Result<T> : Result
{
    private readonly T _value;

    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess 
        ? _value 
        : throw new InvalidOperationException("Cannot access Value of a failed result.");

    public static Result<T> Success(T value) => new(true, value, Error.None);

    public static Result<T> Failure(Error error) => new(false, default!, error);
}