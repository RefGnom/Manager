namespace Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;

public class InvalidStatusException(string message) : IntentionalException(message);