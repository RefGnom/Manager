using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Factories;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.UnitTestsCore;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests.Factories;

public class HashedAuthorizationModelFactoryTest : UnitTestBase
{
    private readonly HashedAuthorizationModelFactory hashedAuthorizationModelFactory = new();

    [Test]
    public void TestCreateHashedModel()
    {
        // Arrange
        var authorizationModelDto = Fixture.Create<AuthorizationModelDto>();
        const string hash = "hash";

        // Act
        var authorizationModelWithApiKeyHashDto = hashedAuthorizationModelFactory.CreateHashedModel(
            authorizationModelDto,
            hash
        );

        // Assert
        authorizationModelWithApiKeyHashDto.Should().BeEquivalentTo(authorizationModelDto);
        authorizationModelWithApiKeyHashDto.ApiKeyHash.Should().Be(hash);
    }
}