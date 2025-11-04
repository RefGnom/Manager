namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

public class HelpCommandRequestFactory : ICommandRequestFactory
{
    public ICommandRequest Create(long telegramId, string userInput) => new HelpCommandRequest();

    public string CommandName => "/help";
}