using System.Threading.Tasks;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandExecutor
{
    bool CanExecute(IToolCommand command);
    Task ExecuteAsync(CommandContext context);
}