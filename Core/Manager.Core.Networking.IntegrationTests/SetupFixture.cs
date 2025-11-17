using System.Reflection;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Manager.Core.Networking.IntegrationTests;

public class SetupFixture : SetupFixtureBase
{
    protected override Assembly TargetTestingAssembly => typeof(Program).Assembly;

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.WithRealLogger();
    }
}