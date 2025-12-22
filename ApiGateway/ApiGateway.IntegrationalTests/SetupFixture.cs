using System.Collections.Generic;
using System.Reflection;
using Manager.ApiGateway.Server;
using Manager.Core.Caching;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.ApiGateway.IntegrationalTests;

public class ApiGatewaySetupFixture : SetupFixtureBase
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
        var builder = WebApplication.CreateBuilder();
        serviceCollection.AddDistributedCache(builder.Configuration);
    }
}