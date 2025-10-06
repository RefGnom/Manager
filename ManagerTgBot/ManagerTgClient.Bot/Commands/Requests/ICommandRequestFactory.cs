namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public interface ICommandRequestFactory
{
    HelpCommandRequest CreateHelpRequest();
    StartTimerRequest CreateStartTimerRequest(string userInput);
}