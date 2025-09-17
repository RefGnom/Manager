namespace Manager.Core.Common.HelperObjects.Result;

public class SuccessResultException<TError>(
    TError? error
) : IntentionalException(error?.ToString() ?? "Unknow error");