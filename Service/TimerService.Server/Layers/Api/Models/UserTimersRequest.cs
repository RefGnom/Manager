using System;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для получения таймеров пользователя
/// </summary>
public class UserTimersRequest
{
    public required Guid UserId { get; set; }
    public required bool WithArchived { get; set; }
    public required bool WithDeleted { get; set; }
}