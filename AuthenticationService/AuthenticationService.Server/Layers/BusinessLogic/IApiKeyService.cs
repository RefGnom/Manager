using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IApiKeyService
{
    string CreateApiKey(Guid authorizationModelId);
    Guid ExtractAuthorizationModelId(string apiKey);
    string HashApiKey(string apiKey);
    bool VerifyHashedApiKey(string hashedApiKey, string providedApiKey);
}

public class ApiKeyService(
    IPasswordHasher<ApiKeyService> passwordHasher
) : IApiKeyService
{
    private const int ApiKeyLength = 36;
    private const char ApiKeySeparator = ':';

    public string CreateApiKey(Guid authorizationModelId)
    {
        var apiKeyBytes = RandomNumberGenerator.GetBytes(ApiKeyLength);
        var apiKeyBase64 = Convert.ToBase64String(apiKeyBytes);
        var authModelIdBase64 = Convert.ToBase64String(authorizationModelId.ToByteArray());
        return $"{authModelIdBase64}{ApiKeySeparator}{apiKeyBase64}";
    }

    public Guid ExtractAuthorizationModelId(string apiKey)
    {
        return new Guid(Convert.FromBase64String(SplitApiKey(apiKey).AuthorizationModelIdBase64));
    }

    public string HashApiKey(string apiKey)
    {
        var apiKeyValue = SplitApiKey(apiKey).ApiKey;
        return passwordHasher.HashPassword(this, apiKeyValue);
    }

    public bool VerifyHashedApiKey(string hashedApiKey, string providedApiKey)
    {
        var apiKeyValue = SplitApiKey(providedApiKey).ApiKey;
        var verificationResult = passwordHasher.VerifyHashedPassword(this, hashedApiKey, apiKeyValue);
        return verificationResult != PasswordVerificationResult.Failed;
    }

    private static (string AuthorizationModelIdBase64, string ApiKey) SplitApiKey(string apiKey)
    {
        var parts = apiKey.Split(ApiKeySeparator);
        return (parts[0], parts[1]);
    }
}