namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public class CommandRequestFactory : ICommandRequestFactory
{
    public HelpCommandRequest CreateHelpRequest()
    {
        return new HelpCommandRequest();
    }

    public StartTimerRequest CreateStartTimerRequest(string userInput)
    {
        return new StartTimerRequest();
    }
}