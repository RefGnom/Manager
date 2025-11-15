using System.ComponentModel;

namespace Manager.RecipientService.Server.Implementation.Domain;

public enum RecipientAuthorizationStatus
{
    [Description("Доступ разрешён")]
    Success = 0,

    [Description("Доступ запрещён, т.к. аккаунт не в активном состоянии")]
    AccountIsNotActive = 1,

    [Description("Доступ к сервису запрещён")]
    AccessToServiceDenied = 2,

    [Description("Доступ к ресурсу запрещён")]
    AccessToResourceDenied = 3,
}