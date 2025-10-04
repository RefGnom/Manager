using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.ManagerTgClient.Bot.Configuration;

public static class ApplicationConfigurator
{
    public static IConfigurationManager CreateConfiguration()
    {
        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()
            .Build();
        return configuration;
    }

    public static IServiceProvider CreateServiceProvider(this IConfigurationManager configurationManager) =>
        new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .ConfigureOptionsWithValidation<ManagerBotOptions>()
            .AddSingleton<IConfiguration>(configurationManager)
            .BuildServiceProvider();
}