namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface ICommandParser
{
    string ParseCommand(string userInput);
}