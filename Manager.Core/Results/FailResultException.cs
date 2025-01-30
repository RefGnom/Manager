namespace Manager.Core.Results;

public class FailResultException(string message) : IntentionalException(message);