using System;
using Microsoft.AspNetCore.Mvc;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для остановки таймера, содержит в себе уникальный индекс и время остановки таймера
/// </summary>
public class StopTimerRequest
{
    [FromRoute]
    public required Guid RecipientId { get; set; }

    [FromRoute]
    public required string TimerName { get; set; }

    [FromBody]
    public required DateTime StopTime { get; set; }
}