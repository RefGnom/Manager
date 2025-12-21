using System;

namespace Manager.TimerService.Client.ServiceModels;

public record StopTimerRequest(
    Guid UserId,
    string Name,
    DateTime StopTime
);