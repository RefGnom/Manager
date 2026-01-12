using Manager.Core.AppConfiguration;
using Manager.Core.Common.DependencyInjection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore.Configuration;
using Manager.Core.Logging.Configuration;
using Manager.Core.Networking;
using Manager.ManagerTgClient.Bot.Layers.Api.States;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Application.Configuration;

public static class ApplicationConfigurator
{
    public static IConfigurationManager CreateConfiguration()
    {
        SolutionRootEnvironmentVariablesLoader.Load();
        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()
            .AddJsonFile("appsettigns.json")
            .AddEnvironmentVariables()
            .Build();
        return configuration;
    }

    public static IServiceProvider CreateServiceProvider(this IConfigurationManager configurationManager) =>
        new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .UseAutoRegistrationForCoreNetworking()
            .ConfigureOptionsWithValidation<ManagerBotOptions>()
            .AddSingleton<IConfiguration>(configurationManager)
            .AddSingleton<IRequestFactory, RequestFactory>()
            .AddSingleton<IResilientHttpClientFactory, ResilientHttpClientFactory>()
            .AddSingleton<ITimerServiceApiClientFactory, TimerServiceApiClientFactory>()
            .AddSingleton<ITimerServiceApiClient>(x =>
                x.GetRequiredService<ITimerServiceApiClientFactory>()
                    .Create(configurationManager.GetValue<string>("TimerServiceApiKey")!)
            )
            .AddSingleton<ITelegramBotClient>(x =>
                new TelegramBotClient(x.GetRequiredService<IOptions<ManagerBotOptions>>().Value.ManagerTgBotToken)
            )
            .AddSingleton<Lazy<IStateProvider>>(x => new Lazy<IStateProvider>(x.GetRequiredService<IStateProvider>))
            .UseNpg()
            .AddLogging(x => x.AddConsole())
            .AddCustomLogger(configurationManager, "Development")
            .BuildServiceProvider();
}