namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public class CreateUserRequest(
    string userName,
    long telegramId
) : ICommandRequest
{
    public string UserName = userName;
    public long TelegramId = telegramId;
}