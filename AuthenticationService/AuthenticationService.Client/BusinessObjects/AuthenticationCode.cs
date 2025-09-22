using System.ComponentModel;

namespace Manager.AuthenticationService.Client.BusinessObjects;

public enum AuthenticationCode
{
    [Description("Неизвестно")]
    Unknown = 0,
    [Description("Апи ключ аутентифицирован")]
    Authenticated = 1,
    [Description("Апи ключ не найден")]
    ApiKeyNotFound = 2,
    [Description("Ресурс недоступен для данного апи ключа")]
    ResourceNotAvailable = 3,
    [Description("Апи ключ не действующий")]
    ApiKeyInactive = 4,
}