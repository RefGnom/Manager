using System;
using System.Text.Json.Serialization;

namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public class PatchRecipientAccountRequest
{
    [JsonIgnore]
    public required Guid RecipientId { get; init; }
    public string? NewLogin { get; init; }
    public string? NewPassword { get; init; }
    public int? NewRecipientTimeUtcOffsetHours { get; init; }
}