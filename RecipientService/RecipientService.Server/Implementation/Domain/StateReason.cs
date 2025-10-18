using System.ComponentModel;

namespace Manager.RecipientService.Server.Implementation.Domain;

public enum StateReason
{
    [Description("Неизвестно")]
    Unknown,

    [Description("Новый пользователь")]
    NewUser,

    [Description("Активирован администратором")]
    ActivatedByAdmin,

    [Description("Восстановлен по запросу пользователя")]
    RestoredByUserRequest,

    [Description("Разблокированный")]
    Unbanned,

    [Description("Удалён по запросу пользователя")]
    DeletedByUserRequest,

    [Description("Заблокированный")]
    Banned,
}