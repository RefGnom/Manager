using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelWithApiKeyDto(
    Guid Id,
    string ApiKey,
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    AuthorizationModelState State,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
) : AuthorizationModelDto(
    Id,
    Owner,
    AvailableServices,
    AvailableResources,
    State,
    CreatedUtcTicks,
    ExpirationUtcTicks
);