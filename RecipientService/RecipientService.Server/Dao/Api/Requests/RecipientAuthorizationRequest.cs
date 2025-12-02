using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class RecipientAuthorizationRequest
{
    [Required]
    public required Guid RecipientId { get; init; }

    [Required]
    public required string RequestedService { get; init; }

    [Required]
    public required string RequestedResource { get; init; }
}