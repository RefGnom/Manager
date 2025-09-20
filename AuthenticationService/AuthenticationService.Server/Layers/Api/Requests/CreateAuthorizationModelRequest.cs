using System;
using System.ComponentModel.DataAnnotations;
using Manager.Core.Common.Validation.DataAnnotationsCustom;

namespace Manager.AuthenticationService.Server.Layers.Api.Requests;

public class CreateAuthorizationModelRequest
{
    [Required]
    public required string Owner { get; init; }

    [Required]
    public required string[] AvailableServices { get; init; }

    [Required]
    public required string[] AvailableResources { get; init; }

    [DateTimeFromUtcNow]
    public DateTime? ExpirationDateUtc { get; init; }
}