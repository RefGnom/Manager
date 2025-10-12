using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.AuthenticationService.IntegrationTests;

public class AuthorizationModelRepositoryTest : IntegrationTestBase
{
    private IAuthorizationModelRepository AuthorizationModelRepository =>
        ServiceProvider.GetRequiredService<IAuthorizationModelRepository>();

    [Test]
    public async Task TestCreate()
    {
        // Arrange
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();

        // Act
        await AuthorizationModelRepository.CreateAsync(authorizationModel);

        // Assert
        var foundAuthorizationModel = await AuthorizationModelRepository.FindAsync(authorizationModel.Id);
        foundAuthorizationModel.Should().NotBeNull();
        foundAuthorizationModel.Should().BeEquivalentTo(authorizationModel);
    }

    [Test]
    public async Task TestUpdateHash_ShouldNotBeUpdated()
    {
        // Arrange
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();
        await AuthorizationModelRepository.CreateAsync(authorizationModel);

        var foundAuthorizationModel = await AuthorizationModelRepository.ReadAsync(authorizationModel.Id);
        var newAuthorizationModel = foundAuthorizationModel with { ApiKeyHash = "new hash" };

        // Act
        await AuthorizationModelRepository.UpdateAsync(newAuthorizationModel);

        // Assert
        var updatedAuthorizationModel = await AuthorizationModelRepository.FindAsync(authorizationModel.Id);
        updatedAuthorizationModel.Should().NotBeNull();
        updatedAuthorizationModel.Should().BeEquivalentTo(foundAuthorizationModel);
    }

    [Test]
    public async Task TestUpdateServiceResources_ShouldBeUpdated()
    {
        // Arrange
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();
        await AuthorizationModelRepository.CreateAsync(authorizationModel);

        var foundAuthorizationModel = await AuthorizationModelRepository.ReadAsync(authorizationModel.Id);
        var newAuthorizationModel = foundAuthorizationModel with
        {
            AvailableServices = ["service1", "service2"],
            AvailableResources = ["resource1", "resource2", "resource3"],
        };

        // Act
        await AuthorizationModelRepository.UpdateAsync(newAuthorizationModel);

        // Assert
        var updatedAuthorizationModel = await AuthorizationModelRepository.FindAsync(authorizationModel.Id);

        updatedAuthorizationModel.Should().NotBeNull();
        updatedAuthorizationModel.Should().BeEquivalentTo(newAuthorizationModel);
    }

    [Test]
    public async Task TestFindByAuthorizationModelBody()
    {
        // Arrange
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();
        await AuthorizationModelRepository.CreateAsync(authorizationModel);

        // Act
        var foundAuthorizationModel = await AuthorizationModelRepository.FindAsync(
            authorizationModel.Owner,
            authorizationModel.AvailableServices,
            authorizationModel.AvailableResources
        );

        // Assert
        foundAuthorizationModel.Should().NotBeNull();
        foundAuthorizationModel.Should().BeEquivalentTo(authorizationModel);
    }

    [Test]
    public async Task TestDelete()
    {
        // Arrange
        var authorizationModel = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();
        await AuthorizationModelRepository.CreateAsync(authorizationModel);

        var foundAuthorizationModel = await AuthorizationModelRepository.ReadAsync(authorizationModel.Id);

        // Act
        await AuthorizationModelRepository.DeleteAsync(foundAuthorizationModel);

        // Assert
        var deletedAuthorizationModel = await AuthorizationModelRepository.FindAsync(authorizationModel.Id);
        deletedAuthorizationModel.Should().BeNull();
    }

    [Test]
    public async Task TestSelectByExpirationTicks()
    {
        // Arrange
        var authorizationModelsTicks = new[] { 5, 6, 7, 8, 9, 10, 11 };
        var authorizationModels = authorizationModelsTicks.Select(ticks => Fixture
            .Build<AuthorizationModelWithApiKeyHashDbo>()
            .With(x => x.ExpirationUtcTicks, ticks)
            .With(x => x.State, AuthorizationModelState.Active)
            .Create()
        ).ToArray();

        await DataContext.InsertRangeAsync(authorizationModels);

        // Act
        var authorizationModelDbos = await AuthorizationModelRepository.SelectByExpirationTicksAsync(9);

        // Assert
        authorizationModelDbos.Should().HaveCountGreaterThanOrEqualTo(5);
        authorizationModelDbos.Select(x => x.Id)
            .Should()
            .Contain(authorizationModels.Take(5).Select(x => x.Id));
    }

    [Test]
    public async Task TestSetStatus()
    {
        // Arrange
        var authorizationModels = Enum.GetValues<AuthorizationModelState>().Select(state => Fixture
            .Build<AuthorizationModelWithApiKeyHashDbo>()
            .With(x => x.State, state)
            .Create()
        ).ToArray();
        var authorizationModelIds = authorizationModels.Select(x => x.Id).ToArray();

        await DataContext.InsertRangeAsync(authorizationModels);

        // Act
        await AuthorizationModelRepository.SetStatusAsync(AuthorizationModelState.Revoked, authorizationModelIds);

        // Assert
        var selectedAuthorizationModels = await DataContext.SelectAsync<AuthorizationModelWithApiKeyHashDbo, Guid>(
            x => x.Id,
            authorizationModelIds
        );
        selectedAuthorizationModels.Should().HaveCount(authorizationModels.Length);
        var uniqueStates = selectedAuthorizationModels.Select(x => x.State).Distinct().ToArray();
        uniqueStates.Should().HaveCount(1);
        uniqueStates[0].Should().Be(AuthorizationModelState.Revoked);
    }
}