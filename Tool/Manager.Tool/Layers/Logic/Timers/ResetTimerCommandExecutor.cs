using System;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class ResetTimerCommandExecutor(IToolCommandFactory toolCommandFactory) : CommandExecutorBase<ResetTimerCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        throw new NotImplementedException();
    }
}