namespace Manager.ManagerTgClient.Bot.Services;

public class CommandParser : ICommandParser
{
    public string ParseCommand(string userInput) => userInput.Split(' ')[0];
}