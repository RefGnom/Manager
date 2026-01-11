using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface IBotInteractionService
{
    Task SendMessageAsync(long userId, string message, ReplyMarkup? replyMarkup = null);
    Task AnswerCallbackQueryAsync(string callbackQueryId);
    Task SendWarningMessage(long userId, string message, ReplyMarkup? replyMarkup);
    Task SendErrorMessage(long userId, string message, ReplyMarkup? replyMarkup);
}