namespace Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;

public class NotFoundException(
    string message
) : IntentionalException(message);