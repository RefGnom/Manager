namespace Manager.ManagerTgClient.Bot.Layers.Services;

public class CommandParser : ICommandParser
{
    public string ParseCommand(string userInput) => userInput.Split(' ')[0];
}