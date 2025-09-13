using System.Threading.Tasks;
using Manager.Tool.Configuration;
using Manager.Tool.Layers.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Tool;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var serviceProvider = DependencyInjectionConfiguration.ConfigureServiceCollection();
        var managerTool = serviceProvider.GetRequiredService<IManagerTool>();
        await managerTool.RunAsync(args);
    }
}