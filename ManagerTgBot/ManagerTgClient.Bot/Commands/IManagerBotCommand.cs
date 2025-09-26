using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Commands;

public interface IManagerBotCommand
{
    Task ExecuteAsync(ITelegramBotClient botClient, long chatId);
}