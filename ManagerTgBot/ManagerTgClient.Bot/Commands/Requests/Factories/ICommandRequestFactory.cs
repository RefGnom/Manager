namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public interface ICommandRequestFactory
{
    string CommandName { get; }
    ICommandRequest Create(long telegramId, string userInput);
}