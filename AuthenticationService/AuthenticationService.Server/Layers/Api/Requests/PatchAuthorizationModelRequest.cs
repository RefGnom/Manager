using System;
using System.ComponentModel.DataAnnotations;
using Manager.Core.Common.Validation.DataAnnotationsCustom;

namespace Manager.AuthenticationService.Server.Layers.Api.Requests;

public class PatchAuthorizationModelRequest
{
    [Required]
    public Guid AuthorizationModelId { get; set; }

    public string? Owner { get; init; }
    public string[]? AvailableServices { get; init; }
    public string[]? AvailableResources { get; init; }

    [DateTimeFromUtcNow]
    public DateTime? ExpirationDateUtc { get; init; }
}