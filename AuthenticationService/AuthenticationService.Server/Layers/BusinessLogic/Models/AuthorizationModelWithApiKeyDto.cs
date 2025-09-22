using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelWithApiKeyDto(
    Guid Id,
    string ApiKey,
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    bool IsRevoked,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
) : AuthorizationModelDto(
    Id,
    Owner,
    AvailableServices,
    AvailableResources,
    IsRevoked,
    CreatedUtcTicks,
    ExpirationUtcTicks
);