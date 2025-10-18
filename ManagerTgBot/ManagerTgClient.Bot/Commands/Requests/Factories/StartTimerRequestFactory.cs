namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public class StartTimerRequestFactory : ICommandRequestFactory
{
    public ICommandRequest Create(long telegramId, string userInput) => throw new NotImplementedException();

    public string CommandName => "/StartTimer";
}