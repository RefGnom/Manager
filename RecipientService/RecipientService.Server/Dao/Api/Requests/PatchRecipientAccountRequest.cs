using System;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class PatchRecipientAccountRequest
{
    public Guid RecipientId { get; init; }

    public string? NewLogin { get; init; }

    public string? NewPassword { get; init; }

    public int? NewRecipientTimeUtcOffsetHours { get; init; }
}