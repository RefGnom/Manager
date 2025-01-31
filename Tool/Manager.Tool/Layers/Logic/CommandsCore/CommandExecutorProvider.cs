namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandExecutorProvider(IEnumerable<ICommandExecutor> executors) : ICommandExecutorProvider
{
    private readonly ICommandExecutor[] _executors = executors.ToArray();

    public ICommandExecutor? GetByContext(CommandContext context)
    {
        return _executors.FirstOrDefault(x => x.CanExecute(context));
    }
}