using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommandExecutor(IToolCommandFactory toolCommandFactory) : CommandExecutorBase<SelectUserTimersCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        throw new System.NotImplementedException();
    }
}