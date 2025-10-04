using System.ComponentModel.DataAnnotations;

namespace Manager.ManagerTgClient.Bot.Configuration;

public class ManagerBotOptions
{
    [Required]
    public required string ManagerTgBotToken { get; init; }
}