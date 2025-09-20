using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelDto(
    Guid Id,
    string ApiKey,
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
);