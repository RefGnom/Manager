using System;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для запуска таймера.
///     Содержит в себе уникальный индекс таймера, время старта и проме
/// </summary>
public class StartTimerRequest
{
    public required DateTime StartTime { get; set; }
    public TimeSpan? PingTimeout { get; set; }
}