using System.ComponentModel.DataAnnotations;


namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class PatchRecipientAccountRequest
{
    public string? NewLogin { get; init; }

    public string? NewPassword { get; init; }

    [Range(-12, 14, ErrorMessage = "Неправильное смещение времени от всемирного времени UTC")]
    public int? NewRecipientTimeUtcOffsetHours { get; init; }
}