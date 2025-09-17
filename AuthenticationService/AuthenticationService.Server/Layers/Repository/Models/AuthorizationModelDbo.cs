using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.AuthenticationService.Server.Layers.Repository.Models;

[Table("authorizationModels")]
public class AuthorizationModelDbo
{
    [Column("id"), Required, Key]
    public required Guid Id { get; set; }

    [Column("apiKeyHash"), Required]
    public required string ApiKeyHash { get; set; }

    [Column("availableServices"), Required]
    public required string[] AvailableServices { get; set; }

    [Column("availableResources"), Required]
    public required string[] AvailableResources { get; set; }

    [Column("createdUtcTicks"), Required]

    public required long CreatedUtcTicks { get; set; }

    [Column("expirationUtcTicks")]
    public long? ExpirationUtcTicks { get; set; }
}