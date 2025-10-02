using System;

namespace Manager.Tool.Configuration;

public static class Environment
{
    public const string Debug = "Debug";
    public const string Information = "Information";

    private static bool initialized;
    private static string currentEnvironment = null!;

    public static string CurrentEnvironment
    {
        get => !initialized ? throw new InvalidOperationException("Environment not initialized") : currentEnvironment;
        private set => currentEnvironment = value;
    }

    public static void DefineEnvironment(string environment)
    {
        if (initialized)
        {
            throw new InvalidOperationException("Environment has already been initialized");
        }

        CurrentEnvironment = environment;
        initialized = true;
    }
}