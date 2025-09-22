using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelDto(
    Guid Id,
    string Owner,
    string[] AvailableServices,
    string[] AvailableResources,
    bool IsRevoked,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
);