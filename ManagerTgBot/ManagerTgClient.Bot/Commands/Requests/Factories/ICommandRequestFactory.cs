namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public interface ICommandRequestFactory
{
    Task<ICommandRequest> CreateAsync(string userInput);
    string CommandName { get; }

}