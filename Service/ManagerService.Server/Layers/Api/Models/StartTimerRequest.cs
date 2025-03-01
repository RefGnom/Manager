using System;

namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Запрос для запуска таймера.
/// Содержит в себе уникальный индекс таймера, время старта и проме
/// </summary>
public class StartTimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public TimeSpan PingTimeout { get; set; }
}