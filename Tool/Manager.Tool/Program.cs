using System.Threading.Tasks;
using Manager.Tool.Configuration;
using Manager.Tool.Layers.Logic;
using Manager.Tool.Layers.Logic.CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Tool;

public static class Program
{
    private static async Task Main(string[] args)
    {
        Environment.DefineEnvironment(CommandLineArgumentsHelper.GetEnvironment(args));

        var serviceProvider = ToolConfigurator
            .CreateSettingsConfiguration()
            .CreateServiceProvider();
        var managerTool = serviceProvider.GetRequiredService<IManagerTool>();
        await managerTool.RunAsync(args);
    }
}