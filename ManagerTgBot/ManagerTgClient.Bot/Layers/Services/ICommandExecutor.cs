namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface ICommandExecutor
{
    Task ExecuteAsync(string userInput, long chatId);
}