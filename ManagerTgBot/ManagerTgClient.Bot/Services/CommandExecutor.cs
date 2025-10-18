using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Services;

public class ManagerCommandExecutor(
    ICommandResolver commandResolver,
    ITelegramBotClient botClient
) : ICommandExecutor
{
    public async Task ExecuteAsync(string userInput, long chatId)
    {
        var resolverData = commandResolver.Resolve(userInput);
        var command = resolverData.Command;
        var requestFactory = resolverData.Factory;
        var request = await requestFactory.CreateAsync(chatId, userInput);
        var commandResult = await command.ExecuteAsync(request);
        await botClient.SendMessage(chatId, commandResult.Message);
    }
}