namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;

public interface IStartTimerRequestBuilder
{
    IStartTimerRequestBuilder WithName(string name);
    IStartTimerRequestBuilder WithDescription(string description);
}