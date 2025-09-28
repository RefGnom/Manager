using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthorizationModelRepositoryTest : AuthenticationServiceTestBase
{
    private IAuthorizationModelRepository authorizationModelRepository = null!;

    [SetUp]
    public void Setup()
    {
        authorizationModelRepository = ServiceProvider.GetRequiredService<IAuthorizationModelRepository>();
    }

    [Test]
    public async Task Test1()
    {
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDbo>();
        await authorizationModelRepository.CreateAsync(authorizationModel);

        var foundAuthorizationModel = await authorizationModelRepository.FindAsync(authorizationModel.Id);

        foundAuthorizationModel.Should().NotBeNull();
        foundAuthorizationModel.Should().BeEquivalentTo(authorizationModel);
    }
}