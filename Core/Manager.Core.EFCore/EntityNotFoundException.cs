using Manager.Core.Common.HelperObjects;

namespace Manager.Core.EFCore;

public class EntityNotFoundException(
    string message
) : IntentionalException(message);