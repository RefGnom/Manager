namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

public interface ICommandRequestFactory
{
    string CommandName { get; }
    ICommandRequest Create(long telegramId, string userInput);
}