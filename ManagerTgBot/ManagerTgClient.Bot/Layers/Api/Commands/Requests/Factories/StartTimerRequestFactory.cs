namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

public class StartTimerRequestFactory : ICommandRequestFactory
{
    public ICommandRequest Create(long telegramId, string userInput) => new StartTimerRequest(telegramId, userInput);

    public string CommandName => "/startTimer";
}