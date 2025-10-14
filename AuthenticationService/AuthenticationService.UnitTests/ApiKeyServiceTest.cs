using System;
using FluentAssertions;
using Manager.AuthenticationService.Server.Layers.BusinessLogic;
using Manager.Core.UnitTestsCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Manager.AuthenticationService.UnitTests;

public class ApiKeyServiceTest : UnitTestBase
{
    private ApiKeyService apiKeyService = null!;

    protected override void SetUp()
    {
        base.SetUp();
        apiKeyService = new ApiKeyService(new PasswordHasher<ApiKeyService>(), NullLogger<ApiKeyService>.Instance);
    }

    [Test]
    public void TestCreateApiKeyAndExtractAuthorizationModelId()
    {
        // Arrange
        var authorizationModelId = Guid.NewGuid();
        var apiKey = apiKeyService.CreateApiKey(authorizationModelId);

        // Act
        var extractAuthorizationModelIdResult = apiKeyService.TryExtractAuthorizationModelId(apiKey);

        // Assert
        extractAuthorizationModelIdResult.IsSuccess.Should().BeTrue();
        extractAuthorizationModelIdResult.Value.Should().Be(authorizationModelId);
    }

    [Test]
    public void TestExtractAuthorizationModelIdWithWrongFormatApiKey_ShouldReturnResultFail()
    {
        // Arrange
        const string apiKey = "wrong api key";

        // Act
        var extractAuthorizationModelIdResult = apiKeyService.TryExtractAuthorizationModelId(apiKey);

        // Assert
        extractAuthorizationModelIdResult.IsFailure.Should().BeTrue();
    }

    [Test]
    public void TestExtractAuthorizationModelIdWithWrongApiKey_ShouldReturnResultFail()
    {
        // Arrange
        const string apiKey = "auth model id:wrong api key";

        // Act
        var extractAuthorizationModelIdResult = apiKeyService.TryExtractAuthorizationModelId(apiKey);

        // Assert
        extractAuthorizationModelIdResult.IsFailure.Should().BeTrue();
    }

    [Test]
    public void TestExtractAuthorizationModelId_CorrectApiKeyIdentifier_ShouldReturnResultSuccess()
    {
        // Arrange
        const string apiKey = "3H1G3INV9kaSTVTsSUKgUg==: какой-то рандомный бред с пробелами ";

        // Act
        var extractAuthorizationModelIdResult = apiKeyService.TryExtractAuthorizationModelId(apiKey);

        // Assert
        extractAuthorizationModelIdResult.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void TestThatHashByApiKeySuccessfullyVerified()
    {
        // Arrange
        var authorizationModelId = Guid.NewGuid();
        var apiKey = apiKeyService.CreateApiKey(authorizationModelId);
        var apiKeyHash = apiKeyService.HashApiKey(apiKey);

        // Act
        var isVerified = apiKeyService.VerifyHashedApiKey(apiKeyHash, apiKey);

        // Assert
        isVerified.Should().BeTrue();
    }

    [Test]
    public void TestThatHashByOtherApiKeyFailureVerified()
    {
        // Arrange
        var apiKey1 = apiKeyService.CreateApiKey(Guid.NewGuid());
        var apiKey2 = apiKeyService.CreateApiKey(Guid.NewGuid());
        var apiKeyHash2 = apiKeyService.HashApiKey(apiKey2);

        // Act
        var isVerified = apiKeyService.VerifyHashedApiKey(apiKeyHash2, apiKey1);

        // Assert
        isVerified.Should().BeFalse();
    }

    [Test]
    public void TestThatHashByRandomApiKeySuccessfullyVerified()
    {
        // Arrange
        const string apiKey = " : какой-то бред с пробелами ";
        var apiKeyHash = apiKeyService.HashApiKey(apiKey);

        // Act
        var isVerified = apiKeyService.VerifyHashedApiKey(apiKeyHash, apiKey);

        // Assert
        isVerified.Should().BeTrue();
    }

    [Test]
    public void TestThatRandomHashStringThrowsException()
    {
        // Arrange
        const string apiKey = " : вроде бы настоящий апи ключ";
        const string apiKeyHash = " какой - то рандомный бред с пробелами";

        // Assert
        apiKeyService.Invoking(x => x.VerifyHashedApiKey(apiKeyHash, apiKey)).Should().Throw();
    }

    [Test]
    [Description("Специальный тест, который никогда не проходит")]
    public void PainTest()
    {
        1.Should().Be(2);
    }
}