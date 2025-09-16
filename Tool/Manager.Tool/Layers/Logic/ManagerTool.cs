using System.Threading.Tasks;
using Manager.Core.Common.Linq;
using Manager.Tool.Layers.Logic.Authentication;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Manager.Tool.Layers.Logic;

public class ManagerTool(
    ICommandContextFactory commandContextFactory,
    ICommandExecutorProvider commandExecutorProvider,
    ILogger<ManagerTool> logger,
    IArgumentsValidator argumentsValidator
) : IManagerTool
{
    public Task RunAsync(string[] arguments)
    {
        var validationResult = argumentsValidator.Validate(arguments);
        if (!validationResult.IsSuccess)
        {
            logger.LogInformation("{message}", validationResult.FailureMessage);
            return Task.CompletedTask;
        }

        var context = commandContextFactory.Create(arguments);
        var jsonContext = JsonConvert.SerializeObject(context);
        logger.LogDebug("Собрали контекст команды\n{context}", jsonContext);

        var commandExecutor = commandExecutorProvider.GetByContext(context);
        if (commandExecutor is not null)
        {
            if (context.User is null && commandExecutor is not AuthenticateCommandExecutor)
            {
                logger.LogInformation("Необходимо выполнить аутентификацию, используя команду \"manager auth --login 'your login'\"");
            }

            return commandExecutor.ExecuteAsync(context);
        }

        logger.LogInformation("Неизвестная команда");
        logger.LogDebug("Не нашли исполнителя для аргументов {arguments}", arguments.JoinToString(", "));
        return Task.CompletedTask;
    }
}