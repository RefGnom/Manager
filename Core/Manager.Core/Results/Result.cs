namespace Manager.Core.Results;

public class Result<T>(T? value, bool isSuccess, string failureMessage)
{
    public T? Value { get; } = value;
    public bool IsSuccess { get; } = isSuccess;
    public string FailureMessage { get; } = failureMessage;

    public T EnsuredValue => IsSuccess && Value is not null ? Value : throw new FailResultException(FailureMessage);
}

public static class Result
{
    public static Result<T> CreateSuccess<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result<T> CreateFailure<T>(string failureMessage)
    {
        return new Result<T>(default, false, failureMessage);
    }
}