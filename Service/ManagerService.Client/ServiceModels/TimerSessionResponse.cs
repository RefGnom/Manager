using System;

namespace ManagerService.Client.ServiceModels;

public class TimerSessionResponse
{
    public required DateTime StartTime { get; set; }
    public required DateTime? StopTime { get; set; }
    public required bool IsOver { get; set; }
}