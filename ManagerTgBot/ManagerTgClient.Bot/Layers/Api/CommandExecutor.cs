using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Layers.Api;

public class CommandExecutor(
    ITelegramBotClient botClient
) : ICommandExecutor
{
    public async Task SendMessageAsync(long userId, string message) => await botClient.SendMessage(userId, message);
    public Task StartTimerAsync(StartTimerRequest request) => throw new NotImplementedException();
}