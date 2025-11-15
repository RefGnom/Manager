using System;

namespace Manager.RecipientService.Client.BusinessObjects.Responses;

public record RecipientAuthorizationResponse(
    Guid RecipientId,
    string RequestedService,
    string RequestedResource,
    RecipientAuthorizationStatus RecipientAuthorizationStatus
);