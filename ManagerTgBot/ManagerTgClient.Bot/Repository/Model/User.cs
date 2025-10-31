using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.ManagerTgClient.Bot.Repository.Model;

[Table("user")]
public class User(
    long telegramId,
    Guid recipientId,
    string userName
)
{
    [Key]
    [Column("id")]
    public long TelegramId { get; init; } = telegramId;
    [Required]
    [Column("recipientId")]
    public Guid RecipientId { get; init; } = recipientId;
    [Required]
    [Column("userName")]
    public string UserName { get; init; } = userName;
}