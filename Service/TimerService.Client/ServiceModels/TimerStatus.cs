using System.ComponentModel;

namespace Manager.TimerService.Client.ServiceModels;

public enum TimerStatus
{
    [Description("Создан")] Created,
    [Description("Запущен")] Started,
    [Description("Остановлен")] Stopped,
    [Description("Сброшен")] Reset,
    [Description("Архивный")] Archived,
    [Description("Удалён")] Deleted,
}