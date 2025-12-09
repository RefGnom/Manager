using System;

namespace Manager.TimerService.Client.ServiceModels;

public record CommonTimerRequest(
    Guid UserId,
    string Name
);