using System;

namespace Manager.TimerService.Client.ServiceModels;

public class DeleteTimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
}