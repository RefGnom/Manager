using System;
using System.Text.Json.Serialization;

namespace Manager.TimerService.Server.Layers.Api.Models;

/// <summary>
///     Запрос для запуска таймера.
///     Содержит в себе уникальный индекс таймера, время старта и проме
/// </summary>
public class StartTimerRequest
{
    [JsonIgnore]
    public string? Name { get; set; }

    [JsonIgnore]
    public Guid RecipientId { get; set; }

    public required DateTime StartTime { get; set; }

    public TimeSpan? PingTimeout { get; set; }
}