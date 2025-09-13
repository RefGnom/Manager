namespace Manager.Core.Results;

public class FailResultException<TError>(
    TError? error
) : IntentionalException(error?.ToString() ?? "Unknow error");