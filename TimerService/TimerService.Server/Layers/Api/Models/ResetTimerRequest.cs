using System;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для сброса таймера. Содержит в себе уникальный индекс
/// </summary>
public class ResetTimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
}