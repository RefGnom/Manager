using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class PatchRecipientAccountRequest
{
    [FromRoute, Required]
    public Guid RecipientId { get; init; }

    [FromBody]
    public string? NewLogin { get; init; }

    [FromBody]
    public string? NewPassword { get; init; }

    [FromBody]
    public int? NewRecipientTimeUtcOffsetHours { get; init; }
}