using System;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public record AuthorizationModelDto(
    Guid Id,
    string ApiKeyHash,
    string[] AvailableServices,
    string[] AvailableResources,
    long CreatedUtcTicks,
    long? ExpirationUtcTicks
);