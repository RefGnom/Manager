using System;

namespace Manager.Core.Common.HelperObjects;

public class IntentionalException(
    string message
) : Exception(message);