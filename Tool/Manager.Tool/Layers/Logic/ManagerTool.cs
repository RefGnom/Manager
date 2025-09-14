using System.Threading.Tasks;
using Manager.Core.Common.Linq;
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
    public Task RunAsync(string[] arguments)
    {
        var validationResult = argumentsValidator.Validate(arguments);
        if (!validationResult.IsSuccess)
        {
            userLogger.LogUserMessage(validationResult.FailureMessage);
            return Task.CompletedTask;
        }

        var context = commandContextFactory.Create(arguments);
        var jsonContext = JsonConvert.SerializeObject(context);
        logger.LogInfo(context.IsDebugMode, "Собрали контекст команды\n{0}", jsonContext);

        var commandExecutor = commandExecutorProvider.GetByContext(context);
        if (commandExecutor is not null)
        {
            if (context.User is null && commandExecutor is not AuthenticateCommandExecutor)
            {
                userLogger.LogUserMessage("Необходимо выполнить аутентификацию, используя команду \"manager auth --login 'your login'\"");
            }

            return commandExecutor.ExecuteAsync(context);
        }

        userLogger.LogUserMessage("Неизвестная команда");
        logger.LogWarn(context.IsDebugMode, "Не нашли исполнителя для аргументов {0}", arguments.JoinToString(", "));
        return Task.CompletedTask;
    }
}