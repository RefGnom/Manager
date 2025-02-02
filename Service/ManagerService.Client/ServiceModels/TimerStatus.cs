using System.ComponentModel;

namespace ManagerService.Client.ServiceModels;

public enum TimerStatus
{
    [Description("Создан")] Created,
    [Description("Запущен")] Started,
    [Description("Остановлен")] Stopped,
    [Description("Сброшен")] Reset,
}