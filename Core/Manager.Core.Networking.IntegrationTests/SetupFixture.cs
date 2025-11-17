using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;

namespace Manager.Core.Networking.IntegrationTests;

public class NetworkingSetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(NetworkingSetupFixture).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithRealLogger();
    }
}
