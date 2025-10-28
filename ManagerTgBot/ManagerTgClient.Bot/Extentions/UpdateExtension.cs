using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Extentions;

public static class UpdateExtension
{
    public static long GetUserId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message.From.Id,
            UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
            _ => throw new Exception("Unknown update type"),
        };
    }

    public static string GetUserData(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message.Text,
            UpdateType.CallbackQuery => update.CallbackQuery.Data,
        };
    }
}