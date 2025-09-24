using System.Linq;
using System.Reflection;
using Manager.Core.Common.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.Core.BackgroundTasks;

public static class BackgroundTaskRegistrator
{
    public static IServiceCollection AddBackgroundTasks(this IServiceCollection services, ILogger? logger = null)
    {
        var backgroundTasks = Assembly.GetEntryAssembly()!.GetExportedTypes()
            .Where(x => x.HasInterface<IBackgroundTask>())
            .ToArray();
        logger?.LogInformation("Registration background tasks {tasks}", backgroundTasks.Select(x => x.Name));

        foreach (var taskType in backgroundTasks)
        {
            var taskHandlerType = typeof(BackgroundTaskHandler<>).MakeGenericType(taskType);
            services.AddSingleton(taskType);
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), taskHandlerType));
        }

        return services;
    }
}