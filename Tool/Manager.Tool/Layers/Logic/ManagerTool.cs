using System.Threading.Tasks;
using Manager.Core.Common.Linq;
using Manager.Tool.Layers.Logic.Authentication;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic;

public class ManagerTool(
    ICommandContextFactory commandContextFactory,
    ICommandExecutorProvider commandExecutorProvider,
    ILogger<ManagerTool> logger,
    IToolWriter toolWriter,
    IArgumentsValidator argumentsValidator,
    ICommandMatcher commandMatcher
) : IManagerTool
{
    public Task RunAsync(string[] arguments)
    {
        var validationResult = argumentsValidator.Validate(arguments);
        if (!validationResult.IsSuccess)
        {
            toolWriter.WriteMessage(validationResult.FailureMessage);
            return Task.CompletedTask;
        }

        var context = commandContextFactory.Create(arguments);
        logger.LogDebug("Собрали контекст команды\n{context}", context);

        var mostSuitableCommandResult = commandMatcher.GetMostSuitableForContext(context);
        var mostSuitableCommand = mostSuitableCommandResult.ToolCommand;
        if (!mostSuitableCommandResult.IsFullMath())
        {
            logger.LogDebug("Не нашли исполнителя для аргументов {arguments}", arguments.JoinToString(", "));
            toolWriter.WriteMessage("Возможно вы имели ввиду: {0}", mostSuitableCommand.Example);
            return Task.CompletedTask;
        }

        var commandExecutor = commandExecutorProvider.GetForCommand(mostSuitableCommand);
        if (context.User is null && commandExecutor is not AuthenticateCommandExecutor)
        {
            toolWriter.WriteMessage(
                "Необходимо выполнить аутентификацию, используя команду \"manager auth --login 'your login'\""
            );
        }

        return commandExecutor.ExecuteAsync(context);
    }
}