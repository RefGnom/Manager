using System;

namespace ManagerService.Client.ServiceModels;

public class StartTimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public TimeSpan PingTimeout { get; set; }
}