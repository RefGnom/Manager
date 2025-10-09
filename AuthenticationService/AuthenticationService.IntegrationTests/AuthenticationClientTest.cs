using System.Threading.Tasks;
using Manager.AuthenticationService.Client;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.Core.AppConfiguration.Authentication;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthenticationClientTest : AuthenticationServiceTestBase
{
    private IAuthenticationServiceApiClient authenticationServiceApiClient = null!;

    protected override void CustomizeServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAuthenticationServiceApiClientFactory, AuthenticationServiceApiClientFactory>();
        serviceCollection.AddSingleton<IAuthenticationServiceApiClient>(x
            => x.GetRequiredService<IAuthenticationServiceApiClientFactory>().Create(
                x.GetRequiredService<IOptions<AuthenticationServiceSetting>>().Value.ApiKey
            )
        );
    }

    protected override void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder)
    {
        builder.UseLocalServer();
    }

    [SetUp]
    public void Setup()
    {
        authenticationServiceApiClient = ServiceProvider.GetRequiredService<IAuthenticationServiceApiClient>();
    }

    [Test]
    public async Task TestGetAuthenticationStatus()
    {
        await authenticationServiceApiClient.GetAuthenticationStatusAsync(
            new AuthenticationStatusRequest("Service", "Resource", "ApiKey")
        );
    }
}