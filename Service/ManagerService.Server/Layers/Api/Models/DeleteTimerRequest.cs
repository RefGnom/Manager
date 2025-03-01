namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Запрос для удаления таймера, содержит в себе уникальный индекс
/// </summary>
public class DeleteTimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
}