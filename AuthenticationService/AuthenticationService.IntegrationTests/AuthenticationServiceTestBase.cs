using System.Reflection;
using Manager.AuthenticationService.Server;
using Manager.Core.IntegrationTestsCore;
using Microsoft.Extensions.Configuration;

namespace Manager.AuthenticationService.IntegrationTests;

public abstract class AuthenticationServiceTestBase : IntegrationTestBase
{
    protected override Assembly TargetTestingAssembly { get; } = typeof(Program).Assembly;

    protected override void CustomizeConfiguration(IConfigurationManager configurationManager)
    {
        configurationManager.AddJsonFile("testsettings.json");
    }
}