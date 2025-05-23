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
    IUserLogger userLogger,
    IArgumentsValidator argumentsValidator
) : IManagerTool
{
    private readonly IArgumentsValidator _argumentsValidator = argumentsValidator;
    private readonly ICommandContextFactory _commandContextFactory = commandContextFactory;
    private readonly ICommandExecutorProvider _commandExecutorProvider = commandExecutorProvider;
    private readonly IToolLogger<ManagerTool> _logger = logger;
    private readonly IUserLogger _userLogger = userLogger;

    public Task RunAsync(string[] arguments)
    {
        var validationResult = _argumentsValidator.Validate(arguments);
        if (!validationResult.IsSuccess)
        {
            _userLogger.LogUserMessage(validationResult.FailureMessage);
            return Task.CompletedTask;
        }

        var context = _commandContextFactory.Create(arguments);
        var jsonContext = JsonConvert.SerializeObject(context);
        _logger.LogInfo(context.IsDebugMode, "Собрали контекст команды\n{0}", jsonContext);

        var commandExecutor = _commandExecutorProvider.GetByContext(context);
        if (commandExecutor is not null)
        {
            if (context.User is null && commandExecutor is not AuthenticateCommandExecutor)
            {
                _userLogger.LogUserMessage("Необходимо выполнить аутентификацию, используя команду \"manager auth --login 'your login'\"");
            }

            return commandExecutor.ExecuteAsync(context);
        }

        _userLogger.LogUserMessage("Неизвестная команда");
        _logger.LogWarn(context.IsDebugMode, "Не нашли исполнителя для аргументов {0}", arguments.JoinToString(", "));
        return Task.CompletedTask;
    }
}