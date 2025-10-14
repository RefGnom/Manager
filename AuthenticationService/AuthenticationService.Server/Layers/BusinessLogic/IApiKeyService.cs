using System;
using System.Security.Cryptography;
using Manager.Core.Common.HelperObjects.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic;

public interface IApiKeyService
{
    string CreateApiKey(Guid authorizationModelId);
    Result<Guid, string> TryExtractAuthorizationModelId(string apiKey);
    string HashApiKey(string apiKey);
    bool VerifyHashedApiKey(string hashedApiKey, string providedApiKey);
}

public class ApiKeyService(
    IPasswordHasher<ApiKeyService> passwordHasher,
    ILogger<ApiKeyService> logger
) : IApiKeyService
{
    private const int ApiKeyLength = 36;
    private const char ApiKeySeparator = ':';

    public string CreateApiKey(Guid authorizationModelId)
    {
        logger.LogInformation("Создаём апи ключ для модели авторизации {authModelId}", authorizationModelId);
        var apiKeyBytes = RandomNumberGenerator.GetBytes(ApiKeyLength);
        var apiKeyBase64 = Convert.ToBase64String(apiKeyBytes);
        var authModelIdBase64 = Convert.ToBase64String(authorizationModelId.ToByteArray());
        return $"{authModelIdBase64}{ApiKeySeparator}{apiKeyBase64}";
    }

    public Result<Guid, string> TryExtractAuthorizationModelId(string apiKey)
    {
        var apiKeyParts = apiKey.Split(':');
        if (apiKeyParts.Length != 2)
        {
            logger.LogInformation("У переданного апи ключа нет разделения на authModelId и сам ключ");
            return "Invalid api key format";
        }

        var authorizationModelIdBytes = new byte[16];
        if (Convert.TryFromBase64String(apiKeyParts[0], authorizationModelIdBytes, out var bytesWritten) &&
            bytesWritten == 16)
        {
            return new Guid(authorizationModelIdBytes);
        }

        logger.LogInformation(
            "Не получилось получить authModelId из строки base64, смогли прочитать {bytes} байт",
            bytesWritten
        );
        return "Invalid api key identifier format";
    }

    public string HashApiKey(string apiKey)
    {
        var generatedApiKey = GetGeneratedApiKey(apiKey);
        return passwordHasher.HashPassword(this, generatedApiKey);
    }

    public bool VerifyHashedApiKey(string hashedApiKey, string providedApiKey)
    {
        var generatedApiKey = GetGeneratedApiKey(providedApiKey);
        var verificationResult = passwordHasher.VerifyHashedPassword(this, hashedApiKey, generatedApiKey);
        return verificationResult != PasswordVerificationResult.Failed;
    }

    private static string GetGeneratedApiKey(string apiKey)
    {
        var keyParts = apiKey.Split(ApiKeySeparator);
        if (keyParts.Length != 2)
        {
            throw new ArgumentException(
                "Апи ключ должен быть составным из идентификатора модели авторизации и сгенерированного ключа"
            );
        }

        return keyParts[1];
    }
}