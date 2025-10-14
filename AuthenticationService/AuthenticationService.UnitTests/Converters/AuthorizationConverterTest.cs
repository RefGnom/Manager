using System;
using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.Api.Converters;
using Manager.AuthenticationService.Server.Layers.Api.Requests;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Time;
using Manager.Core.UnitTestsCore;
using NSubstitute;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests.Converters;

public class AuthorizationConverterTest : UnitTestBase
{
    private AuthorizationConverter authorizationConverter = null!;
    private IDateTimeProvider dateTimeProvider = null!;

    protected override void SetUp()
    {
        base.SetUp();
        dateTimeProvider = Substitute.For<IDateTimeProvider>();
        authorizationConverter = new AuthorizationConverter(dateTimeProvider);
    }

    [Test]
    public void TestToDto()
    {
        // Arrange
        var currentDate = DateTime.Today;
        const int daysAlive = 5;
        var createAuthorizationModelRequest = Fixture.Build<CreateAuthorizationModelRequest>()
            .With(x => x.DaysAlive, daysAlive)
            .Create();

        dateTimeProvider.UtcNow.Returns(currentDate);

        // Act
        var createAuthorizationModelDto = authorizationConverter.ToDto(createAuthorizationModelRequest);

        // Assert
        createAuthorizationModelDto.Should().BeEquivalentTo(
            createAuthorizationModelRequest,
            options => options.ExcludingMissingMembers()
        );
        createAuthorizationModelDto.ExpirationDateUtc.Should().Be(currentDate.AddDays(daysAlive));
    }

    [Test]
    public void TestToDtoWithoutDaysAlive()
    {
        // Arrange
        var createAuthorizationModelRequest = Fixture.Build<CreateAuthorizationModelRequest>()
            .Without(x => x.DaysAlive)
            .Create();

        // Act
        var newAuthorizationModelDto = authorizationConverter.ToDto(createAuthorizationModelRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            createAuthorizationModelRequest,
            options => options.ExcludingMissingMembers()
        );
        newAuthorizationModelDto.ExpirationDateUtc.Should().BeNull();
    }

    [Test]
    public void TestMapExpirationUtcTicks()
    {
        // Arrange
        var currentDate = DateTime.Today;
        const int daysAlive = 5;
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Build<PatchAuthorizationModelRequest>().OmitAutoProperties()
            .With(x => x.DaysAlive, daysAlive)
            .Create();

        dateTimeProvider.UtcNow.Returns(currentDate);

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            oldAuthorizationModelDto,
            options => options.Excluding(x => x.ExpirationUtcTicks)
        );
        newAuthorizationModelDto.ExpirationUtcTicks.Should().Be(currentDate.AddDays(daysAlive).Ticks);
    }

    [Test]
    public void TestMapWhenDaysAliveNull()
    {
        // Arrange
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Build<PatchAuthorizationModelRequest>()
            .Without(x => x.DaysAlive)
            .Create();

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.ExpirationUtcTicks.Should().Be(oldAuthorizationModelDto.ExpirationUtcTicks);
    }

    [Test]
    public void TestMapServices()
    {
        // Arrange
        var newServices = new[] { "new service 1", "new service 2" };
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Build<PatchAuthorizationModelRequest>().OmitAutoProperties()
            .With(x => x.AvailableServices, newServices)
            .Create();

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            oldAuthorizationModelDto,
            options => options.Excluding(x => x.AvailableServices)
        );
        newAuthorizationModelDto.AvailableServices.Should().BeEquivalentTo(newServices);
    }

    [Test]
    public void TestMapResources()
    {
        // Arrange
        var newResources = new[] { "new resource 1", "new resource 2" };
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Build<PatchAuthorizationModelRequest>().OmitAutoProperties()
            .With(x => x.AvailableResources, newResources)
            .Create();

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            oldAuthorizationModelDto,
            options => options.Excluding(x => x.AvailableResources)
        );
        newAuthorizationModelDto.AvailableResources.Should().BeEquivalentTo(newResources);
    }

    [Test]
    public void TestMapOwner()
    {
        // Arrange
        const string newOwner = "new owner";
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Build<PatchAuthorizationModelRequest>().OmitAutoProperties()
            .With(x => x.Owner, newOwner)
            .Create();

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            oldAuthorizationModelDto,
            options => options.Excluding(x => x.Owner)
        );
        newAuthorizationModelDto.Owner.Should().BeEquivalentTo(newOwner);
    }

    [Test]
    public void TestFullMap()
    {
        // Arrange
        var currentDate = DateTime.Today;
        var oldAuthorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        var patchRequest = Fixture.Create<PatchAuthorizationModelRequest>();
        dateTimeProvider.UtcNow.Returns(currentDate);

        // Act
        var newAuthorizationModelDto = authorizationConverter.Map(oldAuthorizationModelDto, patchRequest);

        // Assert
        newAuthorizationModelDto.Should().BeEquivalentTo(
            patchRequest,
            options => options.ExcludingMissingMembers()
        );
        newAuthorizationModelDto.ExpirationUtcTicks.Should()
            .Be(currentDate.AddDays(patchRequest.DaysAlive!.Value).Ticks);
        newAuthorizationModelDto.Id.Should().Be(oldAuthorizationModelDto.Id);
        newAuthorizationModelDto.State.Should().Be(oldAuthorizationModelDto.State);
        newAuthorizationModelDto.CreatedUtcTicks.Should().Be(oldAuthorizationModelDto.CreatedUtcTicks);
    }
}