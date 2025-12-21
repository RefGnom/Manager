using System.ComponentModel;

namespace Manager.RecipientService.Client.BusinessObjects;

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