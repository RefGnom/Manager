using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public class BotInteractionService(
    ITelegramBotClient botClient
) : IBotInteractionService
{
    public async Task SendMessageAsync(long userId, string message, ReplyMarkup? replyMarkup = null) =>
        await botClient.SendMessage(userId, message, replyMarkup:  replyMarkup);

    public Task AnswerCallbackQueryAsync(string callbackQueryId) => botClient.AnswerCallbackQuery(callbackQueryId);

    public Task SendWarningMessage(long userId, string message, ReplyMarkup? replyMarkup) =>
        throw new NotImplementedException();

    public Task SendErrorMessage(long userId, string message, ReplyMarkup? replyMarkup) =>
        throw new NotImplementedException();
}