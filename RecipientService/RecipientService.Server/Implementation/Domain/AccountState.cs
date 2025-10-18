using System.ComponentModel;

namespace Manager.RecipientService.Server.Implementation.Domain;

public enum AccountState
{
    [Description("Неизвестно")]
    Unknown,

    [Description("Неактивный")]
    Inactive,

    [Description("Активный")]
    Active,

    [Description("Удалённый")]
    Deleted,

    [Description("Заблокированный")]
    Banned,
}