using System.Threading.Tasks;
using Manager.Core.Extensions.LinqExtensions;
using Manager.Tool.Layers.Logic.Authentication;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;
using Manager.Tool.Layers.Presentation;
using Newtonsoft.Json;

namespace Manager.Tool.Layers.Logic;

public class ManagerTool(
    ICommandContextFactory commandContextFactory,
    ICommandExecutorProvider commandExecutorProvider,
    IToolLogger<ManagerTool> logger,
    IToolWriter toolWriter,
    IArgumentsValidator argumentsValidator,
    ICommandMatcher commandMatcher
) : IManagerTool
{
    private readonly IArgumentsValidator _argumentsValidator = argumentsValidator;
    private readonly ICommandContextFactory _commandContextFactory = commandContextFactory;
    private readonly ICommandExecutorProvider _commandExecutorProvider = commandExecutorProvider;
    private readonly IToolLogger<ManagerTool> _logger = logger;
    private readonly IToolWriter _toolWriter = toolWriter;
    private readonly ICommandMatcher _commandMatcher = commandMatcher;

    public Task RunAsync(string[] arguments)
    {
        var validationResult = _argumentsValidator.Validate(arguments);
        if (!validationResult.IsSuccess)
        {
            _toolWriter.WriteMessage(validationResult.FailureMessage);
            return Task.CompletedTask;
        }

        var context = _commandContextFactory.Create(arguments);
        var jsonContext = JsonConvert.SerializeObject(context);
        _logger.LogInfo(context.IsDebugMode, "Собрали контекст команды\n{0}", jsonContext);

        var mostSuitableCommandResult = _commandMatcher.GetMostSuitableForContext(context);
        var mostSuitableCommand = mostSuitableCommandResult.ToolCommand;
        if (!mostSuitableCommandResult.IsFullMath())
        {
            _logger.LogWarn(context.IsDebugMode, "Не нашли исполнителя для аргументов {0}", arguments.JoinToString(", "));
            _toolWriter.WriteMessage("Возможно вы имели ввиду: {0}", mostSuitableCommand.Example);
            return Task.CompletedTask;
        }

        var commandExecutor = _commandExecutorProvider.GetForCommand(mostSuitableCommand);
        if (context.User is null && commandExecutor is not AuthenticateCommandExecutor)
        {
            _toolWriter.WriteMessage("Необходимо выполнить аутентификацию, используя команду \"manager auth --login 'your login'\"");
        }

        return commandExecutor.ExecuteAsync(context);
    }
}