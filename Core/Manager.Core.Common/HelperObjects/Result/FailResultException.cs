namespace Manager.Core.Common.HelperObjects.Result;

public class FailResultException<TError>(
    TError? error
) : IntentionalException(error?.ToString() ?? "Unknow error");