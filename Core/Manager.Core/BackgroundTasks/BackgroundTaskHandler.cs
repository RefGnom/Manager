using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.Core.BackgroundTasks;

public class BackgroundTaskHandler<TTask>(
    TTask backgroundTask,
    ILogger<BackgroundTaskHandler<TTask>> logger
) : IHostedService where TTask : IBackgroundTask
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = HandleAsync(cancellationToken);
        return Task.CompletedTask;
    }

    private async Task HandleAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var activity = new Activity($"BackgroundTask {backgroundTask.Name}");
            activity.Start();
            try
            {
                logger.LogInformation("Starting background task {name}", backgroundTask.Name);
                await backgroundTask.Execute(cancellationToken);
                logger.LogInformation("Background task {name} completed", backgroundTask.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Background task {name} failed", backgroundTask.Name);
            }
            finally
            {
                activity.Stop();
                activity.Dispose();
            }

            await Task.Delay(backgroundTask.ExecuteInterval, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}