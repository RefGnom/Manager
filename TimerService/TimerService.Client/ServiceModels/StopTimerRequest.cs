using System;

namespace Manager.TimerService.Client.ServiceModels;

public class StopTimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required DateTime StopTime { get; set; }
}