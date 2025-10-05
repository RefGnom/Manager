namespace Manager.ManagerTgClient.Bot.Services;

public class ManagerCommandExecutor(ICommandResolver commandResolver) : ICommandExecutor
{
    public Task ExecuteAsync(string userInput)
    {
        var command = commandResolver.Resolve(userInput);
        await command.ExecuteAsync();
    }
}