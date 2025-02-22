namespace ManagerService.Server.Layers.ServiceLayer.Exceptions;

public class InvalidStatusException(string message) : IntentionalException(message);