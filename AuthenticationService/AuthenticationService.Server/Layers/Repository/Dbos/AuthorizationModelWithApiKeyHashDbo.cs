using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.AuthenticationService.Server.Layers.Repository.Dbos;

public class AuthorizationModelWithApiKeyHashDbo : AuthorizationModelDbo
{
    [Column("apiKeyHash")]
    [Required]
    public required string ApiKeyHash { get; set; }
}