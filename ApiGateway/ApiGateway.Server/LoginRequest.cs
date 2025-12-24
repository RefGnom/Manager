using System.ComponentModel.DataAnnotations;

namespace Manager.ApiGateway.Server;

public class LoginRequest
{
    [Required]
    public required string Login { get; init; }

    [Required]
    public required string Password { get; init; }
}