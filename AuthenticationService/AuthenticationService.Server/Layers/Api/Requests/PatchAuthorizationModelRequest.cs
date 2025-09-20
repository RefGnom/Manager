using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.AuthenticationService.Server.Layers.Api.Requests;

public class PatchAuthorizationModelRequest
{
    [Required]
    public Guid AuthorizationModelId { get; set; }

    public string? Owner { get; init; }
    public string[]? AvailableServices { get; init; }
    public string[]? AvailableResources { get; init; }

    [Range(1, 365)]
    public int? DaysAlive { get; init; }
}