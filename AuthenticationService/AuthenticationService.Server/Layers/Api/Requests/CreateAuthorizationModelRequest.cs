using System.ComponentModel.DataAnnotations;

namespace Manager.AuthenticationService.Server.Layers.Api.Requests;

public class CreateAuthorizationModelRequest
{
    [Required]
    public required string Owner { get; init; }

    [Required]
    public required string[] AvailableServices { get; init; }

    [Required]
    public required string[] AvailableResources { get; init; }

    [Range(1, 365)]
    public int? DaysAlive { get; init; }
}