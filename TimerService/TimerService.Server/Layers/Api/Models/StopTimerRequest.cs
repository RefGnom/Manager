using System;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для остановки таймера, содержит в себе уникальный индекс и время остановки таймера
/// </summary>
public class StopTimerRequest
{
    public required DateTime StopTime { get; set; }
}