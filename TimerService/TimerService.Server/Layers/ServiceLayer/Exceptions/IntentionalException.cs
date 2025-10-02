using System;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;

public class IntentionalException(
    string message
) : Exception(message);