using Manager.Core.Logging.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Manager.Core.IntegrationTestsCore;

public static class IntegrationTestsLoggingConfigurator
{
    public static IServiceCollection AddLogging(
        this IServiceCollection serviceCollection,
        IConfigurationManager configuration,
        bool useNullLogger = true
    )
    {
        if (useNullLogger)
        {
            return serviceCollection
                .AddSingleton<ILogger, NullLogger>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        return serviceCollection
            .AddLogging(x => x.AddConsole())
            .AddCustomLogger(configuration, Environments.Development);
    }
}