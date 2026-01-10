using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders.Factories;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;

public class StartTimerRequestBuilder(IStartTimerRequestFactory factory) : IStartTimerRequestBuilder
{
    private long userId;
    private string? name;
    private string? description;
    public StartTimerRequest Build() => factory.Create(userId, name!, description!);

    public IStartTimerRequestBuilder ForUser(long data)
    {
        userId = data;
        return this;
    }

    public void WithName(string data) => name = data;

    public void WithDescription(string data) => description = data;
}