using Microsoft.Extensions.Configuration;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.ManagerTgClient.Bot.Configuration;

public static class ApplicationConfigurator
{
    public static IConfigurationManager CreateConfiguration()
    {
        return new ConfigurationManager();
    }

    public static IServiceProvider CreateServiceProvider(this IConfigurationManager configurationManager)
    {
        return new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .BuildServiceProvider();
    }
}