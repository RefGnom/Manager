using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manager.ManagerTgClient.Bot.Layers.Repository.Model;

[Table("user")]
public class UserDbo(
    long telegramId,
    Guid recipientId,
    string userName
)
{
    [Key]
    [Column("id")]
    public long UserId { get; init; } = telegramId;

    [Required]
    [Column("recipientId")]
    public Guid RecipientId { get; init; } = recipientId;

    [Required]
    [Column("userName")]
    public string UserName { get; init; } = userName;
}