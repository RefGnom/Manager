using AutoFixture;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.Api.Converters;
using Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;
using Manager.Core.Common.Enum;
using Manager.Core.UnitTestsCore;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests.Converters;

public class AuthenticationConverterTest : UnitTestBase
{
    private readonly AuthenticationConverter authenticationConverter = new();

    [Test]
    public void TestToResponse()
    {
        // Arrange
        var authenticationStatusDto = Fixture.Create<AuthenticationStatusDto>();

        // Act
        var authenticationStatusResponse = authenticationConverter.ToResponse(authenticationStatusDto);

        // Assert
        authenticationStatusResponse.AuthenticationCode.Should().Be(authenticationStatusDto.AuthenticationCode);
        authenticationStatusResponse.AuthenticationCodeMessage
            .Should()
            .Be(authenticationStatusDto.AuthenticationCode.GetDescription());
    }
}