using System;
using ManagerService.Client.ServiceModels;

namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Ответ для запроса, в котором лежат все поля из таймера, которые нужны, для результата в API,
/// а также все сессии таймера и общее время всех сессий
/// </summary>
public class TimerResponse
{
    public required string Name { get; set; }
    public required DateTime? StartTime { get; set; }
    public required TimeSpan ElapsedTime { get; set; }
    public required TimeSpan? PingTimeout { get; set; }
    public required TimerSessionResponse[] Sessions { get; set; }
    public required TimerStatus TimerStatus { get; set; }
}