using System;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommandExecutor(IToolCommandFactory toolCommandFactory) : CommandExecutorBase<DeleteTimerCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        throw new NotImplementedException();
    }
}