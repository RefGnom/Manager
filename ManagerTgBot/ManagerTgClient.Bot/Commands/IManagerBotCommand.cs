namespace Manager.ManagerTgClient.Bot.Commands;

public interface IManagerBotCommand
{
    Task ExecuteAsync(long chatId);
}