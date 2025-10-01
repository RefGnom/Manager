using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Core.BackgroundTasks;

/// <summary>
/// Интерфейс фоновой задачи. Запускается с интервалом `ExecuteInterval`.
/// Фонововые задачи в сервисе регистрируются с помощью метода BackgroundTaskRegistrator.AddBackgroundTasks
/// </summary>
public interface IBackgroundTask
{
    string Name { get; }

    /// <summary>
    /// Интервал запуска процесса. Является динамическим, можно изменять в runtime
    /// </summary>
    TimeSpan ExecuteInterval { get; }

    Task ExecuteAsync(CancellationToken cancellationToken);
}