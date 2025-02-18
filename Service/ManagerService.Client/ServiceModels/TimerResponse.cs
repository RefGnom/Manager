using System;

namespace ManagerService.Client.ServiceModels;

public class TimerResponse
{
    public required string Name { get; set; }
    public required DateTime? StartTime { get; set; }
    public required TimeSpan ElapsedTime { get; set; }
    public required TimeSpan? PingTimeout { get; set; }
    public required TimerSessionResponse[] Sessions { get; set; }
    public required TimerStatus TimerStatus { get; set; }
}