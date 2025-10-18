using System.Collections.Generic;
using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Manager.TimerService.Client;
using Manager.TimerService.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.TimerService.IntegrationTests;

public class TimerSetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(Program).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithLocalServer(
            new Dictionary<string, string>
            {
                ["AuthenticationServiceSetting:ApiKey"] = "random key",
            }
        ).WithRealLogger();
    }

    protected override void CustomizeServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ITimerServiceApiClientFactory, TimerServiceApiClientFactory>();
        serviceCollection.AddSingleton<ITimerServiceApiClient>(x =>
            x.GetRequiredService<ITimerServiceApiClientFactory>().Create("nope")
        );
    }
}