using System.ComponentModel;

namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public enum CreateAuthorizationModelErrorCode
{
    [Description("Неизвестная ошибка")]
    Unknown = 0,

    [Description("Модель авторизации уже существует")]
    AuthorizationModelAlreadyExists = 1,
}