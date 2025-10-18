namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public class CreateUserRequestFactory: ICommandRequestFactory
{
    public string CommandName => "/createuser";

    public Task<ICommandRequest> CreateAsync(string userInput)
    {
        var request = new CreateUserRequest();
        
    }
}