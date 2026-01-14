    namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

    public interface IRequestBuilder<out TRequest>
    {
        TRequest Build();
    }