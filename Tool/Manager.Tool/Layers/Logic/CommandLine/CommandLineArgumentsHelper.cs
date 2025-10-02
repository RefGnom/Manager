using System.Linq;
using Manager.Tool.Configuration;

namespace Manager.Tool.Layers.Logic.CommandLine;

public static class CommandLineArgumentsHelper
{
    private static readonly string[] debugPossibleArgs = ["-d", "--debug", "debug"];

    public static string GetEnvironment(string[] args) =>
        debugPossibleArgs.Any(args.Contains) ? Environment.Debug : Environment.Information;
}