namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandExecutor
{
    Task ExecuteAsync(string userInput, long chatId);
}