using System;

namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Запрос для остановки таймера, содержит в себе уникальный индекс и время остановки таймера
/// </summary>
public class StopTimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
    public required DateTime StopTime { get; set; }
}