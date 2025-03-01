using System;

namespace ManagerService.Client.ServiceModels;

public class TimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
}