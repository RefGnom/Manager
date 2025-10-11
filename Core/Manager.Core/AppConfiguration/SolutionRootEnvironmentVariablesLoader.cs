using System;
using System.IO;

namespace Manager.Core.AppConfiguration;

public static class SolutionRootEnvironmentVariablesLoader
{
    public static void Load()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var rootDir = FindRepositoryRoot(currentDir);

        if (rootDir == null)
        {
            return;
        }

        var envPath = Path.Combine(rootDir, ".env");
        if (!File.Exists(envPath))
        {
            return;
        }

        foreach (var line in File.ReadAllLines(envPath))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith('#'))
            {
                continue;
            }

            var parts = trimmed.Split(['='], 2);
            if (parts.Length != 2)
            {
                continue;
            }

            var key = parts[0].Trim();
            var value = parts[1].Trim().Trim('"', '\'');
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    private static string? FindRepositoryRoot(string startPath)
    {
        var current = new DirectoryInfo(startPath);

        while (current != null)
        {
            if (current.GetFiles("*.sln").Length > 0 || Directory.Exists(Path.Combine(current.FullName, ".git")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return null;
    }
}