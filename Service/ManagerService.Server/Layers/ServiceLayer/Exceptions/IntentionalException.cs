using System;

namespace ManagerService.Server.Layers.ServiceLayer.Exceptions;

public class IntentionalException(string message) : Exception(message);