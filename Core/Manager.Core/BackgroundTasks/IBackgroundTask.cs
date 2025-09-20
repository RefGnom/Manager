using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Core.BackgroundTasks;

public interface IBackgroundTask
{
    string Name { get; }
    TimeSpan ExecuteInterval { get; }
    Task Execute(CancellationToken cancellationToken);
}