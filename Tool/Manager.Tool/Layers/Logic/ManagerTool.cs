using System.Threading.Tasks;
using Manager.Core.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic;

public class ManagerTool(
    ICommandContextFactory commandContextFactory,
    ICommandExecutorProvider commandExecutorProvider,
    IToolLogger<ManagerTool> logger
) : IManagerTool
{
    private readonly ICommandContextFactory _commandContextFactory = commandContextFactory;
    private readonly ICommandExecutorProvider _commandExecutorProvider = commandExecutorProvider;
    private readonly IToolLogger<ManagerTool> _logger = logger;

    public Task RunAsync(string[] arguments)
    {
        var context = _commandContextFactory.Create(arguments);
        var commandExecutor = _commandExecutorProvider.GetByContext(context);
        if (commandExecutor is not null)
        {
            return commandExecutor.ExecuteAsync(context);
        }

        _logger.LogWarn(context.IsDebugMode, "Не нашли исполнителя для аргументов {0}", arguments.JoinToString(", "));
        return Task.CompletedTask;
    }
}