namespace ManagerService.Server.Layers.ServiceLayer.Exceptions;

public class NotFoundException(string message) : IntentionalException(message);