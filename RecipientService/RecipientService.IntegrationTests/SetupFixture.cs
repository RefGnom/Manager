using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Manager.RecipientService.Client;
using Manager.RecipientService.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.RecipientService.IntegrationTests;

public class SetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(Program).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithRealLogger().WithLocalServer();
    }

    protected override void CustomizeServiceCollection(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IRecipientServiceApiClientFactory, RecipientServiceApiClientFactory>();
        serviceCollection.AddSingleton<IRecipientServiceApiClient>(x => x
            .GetRequiredService<IRecipientServiceApiClientFactory>().Create("fake key")
        );
    }
}