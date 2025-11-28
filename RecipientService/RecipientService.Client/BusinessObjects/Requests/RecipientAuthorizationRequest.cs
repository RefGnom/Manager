using System;
using System.Text.Json.Serialization;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record RecipientAuthorizationRequest(
    [property: JsonIgnore] Guid RecipientId,
    string RequestedService,
    string RequestedResource
);