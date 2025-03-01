using System;

namespace ManagerService.Client.ServiceModels;

public class StartTimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public TimeSpan PingTimeout { get; set; }
}