using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Services;

public class ManagerCommandExecutor(ICommandResolver commandResolver) : ICommandExecutor
{
    public async Task ExecuteAsync(ITelegramBotClient botClient,string userInput, long chatId)
    {
        var resolverData = await commandResolver.ResolveAsync(userInput);
        var command = resolverData.Command;
        var requestFactory = resolverData.Factory;
        var request = await requestFactory.CreateAsync(userInput);
        var commandResult =  await command.ExecuteAsync(request);
        await botClient.SendMessage(chatId, commandResult.Value);
    }
}