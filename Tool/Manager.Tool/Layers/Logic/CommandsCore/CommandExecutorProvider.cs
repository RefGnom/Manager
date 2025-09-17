using System.Collections.Generic;
using System.Linq;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class CommandExecutorProvider(IEnumerable<ICommandExecutor> executors) : ICommandExecutorProvider
{
    private readonly ICommandExecutor[] _executors = executors.ToArray();

    public ICommandExecutor GetForCommand(IToolCommand command)
    {
        return _executors.First(x => x.CanExecute(command));
    }
}