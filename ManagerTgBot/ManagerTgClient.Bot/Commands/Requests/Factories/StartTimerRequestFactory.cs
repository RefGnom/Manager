namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public class StartTimerRequestFactory : ICommandRequestFactory
{
    public Task<ICommandRequest> CreateAsync(long telegramId, string userInput) => throw new NotImplementedException();

    public string CommandName => "/StartTimer";
}