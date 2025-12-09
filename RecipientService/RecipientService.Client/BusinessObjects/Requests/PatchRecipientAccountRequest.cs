using System;
using System.Text.Json.Serialization;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record PatchRecipientAccountRequest(
    [property: JsonIgnore]
    Guid RecipientId,
    string? NewLogin,
    string? NewPassword,
    int? NewRecipientTimeUtcOffsetHours
);