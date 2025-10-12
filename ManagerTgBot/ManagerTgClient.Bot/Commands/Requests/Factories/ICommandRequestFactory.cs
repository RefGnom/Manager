namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public interface ICommandRequestFactory
{
    string CommandName { get; }
    Task<ICommandRequest> CreateAsync(string userInput);
}