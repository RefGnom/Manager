using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands;

public abstract class CommandBase : ICommand
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest);

    protected TRequest CastRequest<TRequest>(ICommandRequest request)
        where TRequest : class, ICommandRequest
    {
        var result = request as TRequest;
        if (result is null)
        {
            throw new ArgumentException("Неправильный реквест");
        }

        return result;
    }
}