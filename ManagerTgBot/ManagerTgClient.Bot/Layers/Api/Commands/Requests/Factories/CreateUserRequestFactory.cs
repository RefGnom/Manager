namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

public class CreateUserRequestFactory: ICommandRequestFactory
{
    public string CommandName => "/createUser";

    public ICommandRequest Create(long telegramId, string userInput)
    {
        var request = new CreateUserRequest(userInput, telegramId);
        return request;
    }
}