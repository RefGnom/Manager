namespace Manager.Core.Common.HelperObjects.Result;

public class Result(
    bool isSuccess
)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure { get; } = !isSuccess;
}

public class Result<TError>(
    bool isSuccess,
    TError? error
) : Result(isSuccess)
{
    public TError? Error { get; } = error;

    public static implicit operator Result<TError>(TError error) => new(false, error);

    public static Result<TError> Ok() => new(true, default);

    public static Result<TError> Failure(TError error) => error;
}

public class Result<TValue, TError>(
    bool isSuccess,
    TValue? value,
    TError? error
) : Result(isSuccess)
{
    public TValue Value => IsSuccess && value is not null ? value : throw new FailResultException<TError>(error);
    public TError Error => IsFailure && error is not null ? error : throw new SuccessResultException<TError>(error);

    public static implicit operator Result<TValue, TError>(TError error) => new(false, default, error);

    public static implicit operator Result<TValue, TError>(TValue value) => new(true, value, default);

    public static Result<TValue, TError> Ok(TValue value) => value;

    public static Result<TValue, TError> Failure(TError error) => error;
}