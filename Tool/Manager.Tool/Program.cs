using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.Logging.Configuration;
using Manager.Tool.Layers.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manager.Tool;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var environment = args.Contains("-d") ? "Debug" : "Production";
        var configuration = new ConfigurationManager();
        configuration.AddJsonFile("appsettings.json");
        configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);
        var serviceCollection = new ServiceCollection()
            .AddLogging(x => x.AddConsole())
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon();
        var logger = serviceCollection.AddCustomLogger(configuration, "dfgsdfgdfg");
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var managerTool = serviceProvider.GetRequiredService<IManagerTool>();
        await managerTool.RunAsync(args);
    }
}