using System.Threading.Tasks;

namespace Manager.Tool.Layers.Logic;

public interface IManagerTool
{
    Task RunAsync(string[] arguments);
}