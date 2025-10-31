namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandParser
{
    string ParseCommand(string userInput);
}