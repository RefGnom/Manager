namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Результат запроса для пользователя, в котором хранятся все таймеры этого пользователя
/// </summary>
public class UserTimersResponse
{
    public required TimerResponse[] Timers { get; set; }
}