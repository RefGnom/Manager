using System.ComponentModel.DataAnnotations;

namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class CreateRecipientAccountRequest
{
    [Required]
    public required string Login { get; init; }

    [Required]
    public required string Password { get; init; }

    [Required, Range(-12, 14, ErrorMessage = "Неправильное смещение времени от всемирного времени UTC")]
    public required int RecipientTimeUtcOffsetHours { get; init; }
}