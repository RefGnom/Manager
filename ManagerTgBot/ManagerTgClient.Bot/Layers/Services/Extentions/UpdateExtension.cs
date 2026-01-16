using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Extentions;

public static class UpdateExtension
{
    public static long GetUserId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message!.From!.Id,
            UpdateType.CallbackQuery => update.CallbackQuery!.From!.Id,
            _ => throw new NotSupportedUpdateTypeException("Unknown update type"),
        };
    }

    public static string? GetUserData(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message!.Text,
            UpdateType.CallbackQuery => update.CallbackQuery!.Data,
            _ => throw new NotSupportedUpdateTypeException("Unknow update type"),
        };
    }

    public static int GetMessageId(this Update update) => update.CallbackQuery!.Message!.MessageId;

}