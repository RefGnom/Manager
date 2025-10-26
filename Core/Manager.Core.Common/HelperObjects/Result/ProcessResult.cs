namespace Manager.Core.Common.HelperObjects.Result;

public class ProcessResult<TStatus>(
    bool isSuccess,
    TStatus status
) : Result(isSuccess)
{
    public TStatus Status { get; } = status;
}

public class ProcessResult<TError, TStatus>(
    bool isSuccess,
    TStatus status,
    TError? error
) : ProcessResult<TStatus>(isSuccess, status)
{
    public TError Error => IsFailure && error is not null ? error : throw new SuccessResultException<TError>(error);

    public static ProcessResult<TError, TStatus> Ok(TStatus status) => new(true, status, default);

    public static ProcessResult<TError, TStatus> Failure(TError error, TStatus status) => new(false, status, error);
}

public class ProcessResult<TValue, TError, TStatus>(
    bool isSuccess,
    TStatus status,
    TValue? value,
    TError? error
) : ProcessResult<TStatus>(isSuccess, status)
{
    public TValue Value => IsSuccess && value is not null ? value : throw new FailResultException<TError>(error);
    public TError Error => IsFailure && error is not null ? error : throw new SuccessResultException<TError>(error);

    public static ProcessResult<TValue, TError, TStatus> Ok(TValue value, TStatus status) =>
        new(true, status, value, default);

    public static ProcessResult<TValue, TError, TStatus> Failure(TError error, TStatus status) =>
        new(false, status, default, error);
}