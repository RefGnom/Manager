using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;

namespace Manager.Core.Networking.IntegrationTests;

public class SetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(IPortProvider).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithRealLogger().WithoutDataBase().WithoutAutoRegistration();
    }
}