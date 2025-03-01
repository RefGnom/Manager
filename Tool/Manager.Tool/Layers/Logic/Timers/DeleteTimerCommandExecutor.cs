using System;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory) : CommandExecutorBase<DeleteTimerCommand>(toolCommandFactory)
{
    protected override Task ExecuteAsync(CommandContext context, DeleteTimerCommand deleteTimerCommand)
    {
        throw new NotImplementedException();
    }
}