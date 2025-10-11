namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public class HelpCommandRequestFactory : ICommandRequestFactory
{
    public async Task<ICommandRequest> CreateAsync(string userInput) => await Task.FromResult(new HelpCommandRequest());

    public string CommandName => "/help";
}