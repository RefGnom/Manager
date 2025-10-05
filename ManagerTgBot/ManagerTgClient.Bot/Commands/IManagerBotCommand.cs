using Manager.ManagerTgClient.Bot.Commands.CommandResults;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Commands;

public interface IManagerBotCommand
{
    Task<ICommandResult> ExecuteAsync(ITelegramBotClient botClient, long chatId);
}