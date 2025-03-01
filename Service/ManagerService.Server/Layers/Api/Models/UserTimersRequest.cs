namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Запрос для получения таймеров пользователя
/// </summary>
public class UserTimersRequest
{
    public required User User { get; set; }
    public required bool WithArchived { get; set; }
    public required bool WithDeleted { get; set; }
}