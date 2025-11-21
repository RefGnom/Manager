using System;
using Microsoft.AspNetCore.Mvc;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для запуска таймера.
///     Содержит в себе уникальный индекс таймера, время старта и проме
/// </summary>
public class StartTimerRequest
{
    [FromRoute]
    public required Guid RecipientId { get; init; }

    [FromRoute]
    public required string TimerName { get; init; }

    [FromBody]
    public required DateTime StartTime { get; set; }

    [FromBody]
    public TimeSpan? PingTimeout { get; set; }
}