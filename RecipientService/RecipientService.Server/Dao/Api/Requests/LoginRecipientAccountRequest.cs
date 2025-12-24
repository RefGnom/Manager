using System.ComponentModel.DataAnnotations;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class LoginRecipientAccountRequest
{
    [Required, MinLength(8), MaxLength(100)]
    public required string Login { get; init; }

    [Required, MinLength(8), MaxLength(100)]
    public required string Password { get; init; }
}