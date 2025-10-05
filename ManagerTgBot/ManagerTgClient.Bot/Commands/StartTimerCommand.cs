using Manager.ManagerTgClient.Bot.Commands.CommandResults;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Commands;

public class StartTimerCommand : IManagerBotCommand
{
    public async Task<ICommandResult> ExecuteAsync(ITelegramBotClient botClient, long chatId)
    {
        return await Task.FromResult(new CommandResult("Таймер запущен"));
    }
}