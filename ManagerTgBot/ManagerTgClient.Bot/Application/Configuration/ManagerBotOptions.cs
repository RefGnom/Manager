using System.ComponentModel.DataAnnotations;

namespace Manager.ManagerTgClient.Bot.Application.Configuration;

public class ManagerBotOptions
{
    [Required]
    public required string ManagerTgBotToken { get; init; }
}