using System;

namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
///  Запрос для получения таймера по его уникальному индексу
/// </summary>
public class TimerRequest
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
}