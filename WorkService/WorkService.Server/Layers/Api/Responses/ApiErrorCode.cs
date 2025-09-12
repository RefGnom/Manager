using System.ComponentModel;

namespace WorkService.Server.Layers.Api.Responses;

public enum ApiErrorCode
{
    [Description("Неизвестная ошибка")]
    Unknown = 0,
}