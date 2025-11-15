using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Manager.RecipientService.Server;

namespace Manager.RecipientService.IntegrationTests;

public class SetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(Program).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithRealLogger();
    }
}