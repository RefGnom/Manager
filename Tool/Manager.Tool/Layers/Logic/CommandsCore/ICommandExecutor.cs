using System.Threading.Tasks;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandExecutor
{
    bool CanExecute(CommandContext context);
    Task ExecuteAsync(CommandContext context);
}