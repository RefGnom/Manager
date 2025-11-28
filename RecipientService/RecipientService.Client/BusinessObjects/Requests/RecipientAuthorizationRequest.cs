using System;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record RecipientAuthorizationRequest(
    Guid RecipientId,
    string RequestedService,
    string RequestedResource
);