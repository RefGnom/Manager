using System;
using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Time;
using Manager.Core.UnitTestsCore;
using NSubstitute;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests.Factories;

public class AuthorizationModelFactoryTest : UnitTestBase
{
    private IApiKeyService apiKeyService = null!;
    private AuthorizationModelFactory authorizationModelFactory = null!;
    private IDateTimeProvider dateTimeProvider = null!;

    protected override void SetUp()
    {
        base.SetUp();
        apiKeyService = Substitute.For<IApiKeyService>();
        dateTimeProvider = Substitute.For<IDateTimeProvider>();
        authorizationModelFactory = new AuthorizationModelFactory(apiKeyService, dateTimeProvider);
    }

    [Test]
    public void TestCreateFromModel()
    {
        // Arrange
        const string apiKey = "secret api key";
        var createTicks = DateTime.UtcNow.Ticks;
        var createAuthorizationModelDto = Fixture.Create<CreateAuthorizationModelDto>();

        apiKeyService.CreateApiKey(Arg.Any<Guid>()).Returns(apiKey);
        dateTimeProvider.UtcTicks.Returns(createTicks);

        // Act
        var authorizationModelWithApiKeyDto = authorizationModelFactory.Create(createAuthorizationModelDto);

        // Assert
        authorizationModelWithApiKeyDto.Should().BeEquivalentTo(
            createAuthorizationModelDto,
            options => options.ExcludingMissingMembers()
        );
        authorizationModelWithApiKeyDto.ExpirationUtcTicks.Should()
            .Be(createAuthorizationModelDto.ExpirationDateUtc!.Value.Ticks);
        authorizationModelWithApiKeyDto.ApiKey.Should().Be(apiKey);
        authorizationModelWithApiKeyDto.CreatedUtcTicks.Should().Be(createTicks);
        authorizationModelWithApiKeyDto.State.Should().Be(AuthorizationModelState.Active);
    }

    [Test]
    public void TestCreateFromModelWithNullExpirationDate()
    {
        // Arrange
        var createAuthorizationModelDto = new Fixture().Build<CreateAuthorizationModelDto>()
            .With(x => x.ExpirationDateUtc, (DateTime?)null)
            .Create();

        // Act
        var authorizationModelWithApiKeyDto = authorizationModelFactory.Create(createAuthorizationModelDto);

        // Assert
        authorizationModelWithApiKeyDto.ExpirationUtcTicks.Should().BeNull();
    }

    [Test]
    public void TestCreateFromParameters()
    {
        // Arrange
        const string apiKey = "secret api key";
        var createTicks = DateTime.UtcNow.Ticks;
        const string owner = "model owner";
        var availableServices = new[] { "service 1", "service 2" };
        var availableResources = new[] { "resource 1", "resource 2" };
        const long expirationTicks = 1L;

        apiKeyService.CreateApiKey(Arg.Any<Guid>()).Returns(apiKey);
        dateTimeProvider.UtcTicks.Returns(createTicks);

        // Act
        var authorizationModelWithApiKeyDto = authorizationModelFactory.Create(
            owner,
            availableServices,
            availableResources,
            expirationTicks
        );

        // Assert
        authorizationModelWithApiKeyDto.Owner.Should().Be(owner);
        authorizationModelWithApiKeyDto.AvailableServices.Should().BeEquivalentTo(availableServices);
        authorizationModelWithApiKeyDto.AvailableResources.Should().BeEquivalentTo(availableResources);
        authorizationModelWithApiKeyDto.ExpirationUtcTicks.Should().Be(expirationTicks);
        authorizationModelWithApiKeyDto.ApiKey.Should().Be(apiKey);
        authorizationModelWithApiKeyDto.CreatedUtcTicks.Should().Be(createTicks);
        authorizationModelWithApiKeyDto.State.Should().Be(AuthorizationModelState.Active);
    }

    [Test]
    public void TestCreateFromParametersWithNullExpirationTicks()
    {
        // Arrange

        // Act
        var authorizationModelWithApiKeyDto = authorizationModelFactory.Create(
            Fixture.Create<string>(),
            Fixture.Create<string[]>(),
            Fixture.Create<string[]>(),
            null
        );

        // Assert
        authorizationModelWithApiKeyDto.ExpirationUtcTicks.Should().BeNull();
    }
}