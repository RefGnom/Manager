using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Core.DependencyInjection;

public static class AssemblyProvider
{
    private const string ServiceName = "Manager";
    private static readonly string[] ExtensionTemplates = [".exe", ".dll"];

    public static Assembly[] GetServiceAssemblies()
    {
        return Directory.GetFiles(AppContext.BaseDirectory)
            .Where(IsCorrectExtension)
            .Select(Path.GetFileNameWithoutExtension)
            .Distinct()
            .Where(IsCorrectName)
            .Select(Assembly.Load!)
            .ToArray();
    }

    private static bool IsCorrectExtension(string fileName)
    {
        return ExtensionTemplates.Any(s => fileName.EndsWith(s, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsCorrectName(string? path)
    {
        var fileName = Path.GetFileName(path);
        return fileName is not null && fileName.StartsWith(ServiceName, StringComparison.OrdinalIgnoreCase);
    }
}