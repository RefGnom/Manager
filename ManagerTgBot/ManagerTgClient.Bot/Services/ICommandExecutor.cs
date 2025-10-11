using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandExecutor
{
    Task ExecuteAsync(ITelegramBotClient botClient, string userInput, long chatId);
}