using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Manager.WorkService.Client;
using Manager.WorkService.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.WorkService.IntegrationTests;

public class WorkSetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(Program).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithLocalServer().WithRealLogger();
    }

    protected override void CustomizeServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IWorkServiceApiClientFactory, WorkServiceApiClientFactory>();
        serviceCollection.AddSingleton<IWorkServiceApiClient>(x =>
            x.GetRequiredService<IWorkServiceApiClientFactory>().Create("nope")
        );
    }
}