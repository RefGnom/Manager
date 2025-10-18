using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Services;

public class ManagerCommandExecutor(
    ICommandResolver commandResolver,
    ITelegramBotClient botClient,
    ICommandParser commandParser
) : ICommandExecutor
{
    public async Task ExecuteAsync(string userInput, long chatId)
    {
        var userCommandName = commandParser.ParseCommand(userInput);
        var resolverData = commandResolver.Resolve(userCommandName);
        var command = resolverData.Command;
        var requestFactory = resolverData.Factory;
        var request = requestFactory.Create(chatId, userInput);
        var commandResult = await command.ExecuteAsync(request);
        await botClient.SendMessage(chatId, commandResult.Message);
    }
}