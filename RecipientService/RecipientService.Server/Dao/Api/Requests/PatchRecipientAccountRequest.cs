using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Manager.RecipientService.Server.Dao.Api.Requests;

public class PatchRecipientAccountRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid RecipientId { get; set; }

    public string? NewLogin { get; init; }

    public string? NewPassword { get; init; }

    [Range(-12, 14, ErrorMessage = "Неправильное смещение времени от всемирного времени UTC")]
    public int? NewRecipientTimeUtcOffsetHours { get; init; }
}