using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.AuthenticationService.Server.Layers.Repository.Converters;
using Manager.AuthenticationService.Server.Layers.Repository.Dbos;
using Manager.Core.UnitTestsCore;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests.Converters;

public class AuthorizationModelConverterTest : UnitTestBase
{
    private readonly AuthorizationModelConverter authorizationModelConverter = new();

    [Test]
    public void ToDboExtendedModel()
    {
        // Arrange
        var authorizationModelWithApiKeyHashDto = Fixture.Create<AuthorizationModelWithApiKeyHashDto>();

        // Act
        var authorizationModelWithApiKeyHashDbo =
            authorizationModelConverter.ToDbo(authorizationModelWithApiKeyHashDto);

        // Assert
        authorizationModelWithApiKeyHashDbo.Should().BeEquivalentTo(authorizationModelWithApiKeyHashDto);
    }

    [Test]
    public void ToDtoExtendedModel()
    {
        // Arrange
        var authorizationModelWithApiKeyHashDbo = Fixture.Create<AuthorizationModelWithApiKeyHashDbo>();

        // Act
        var authorizationModelWithApiKeyHashDto =
            authorizationModelConverter.ToDto(authorizationModelWithApiKeyHashDbo);

        // Assert
        authorizationModelWithApiKeyHashDto.Should().BeEquivalentTo(authorizationModelWithApiKeyHashDbo);
    }

    [Test]
    public void ToDbo()
    {
        // Arrange
        var authorizationModelDto = Fixture.Create<AuthorizationModelDto>();

        // Act
        var authorizationModelDbo = authorizationModelConverter.ToDbo(authorizationModelDto);

        // Assert
        authorizationModelDbo.Should().BeEquivalentTo(authorizationModelDto);
    }

    [Test]
    public void ToDto()
    {
        // Arrange
        var authorizationModelDbo = Fixture.Create<AuthorizationModelDbo>();

        // Act
        var authorizationModelDto = authorizationModelConverter.ToDto(authorizationModelDbo);

        // Assert
        authorizationModelDto.Should().BeEquivalentTo(authorizationModelDbo);
    }
}