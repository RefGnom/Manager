using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Core.Common.DependencyInjection.AutoRegistration;

public static class AssemblyProvider
{
    private const string ServiceName = "Manager";
    private static readonly string[] extensionTemplates = [".exe", ".dll"];

    /// <summary>
    ///     Предоставляет все сборки Manager. Возвращает сборки только с переопределённым параметром
    ///     AssemblyName в .csproj, должен быть префикс "Manager"
    /// </summary>
    public static Assembly[] GetServiceAssemblies() => Directory.GetFiles(AppContext.BaseDirectory)
        .Where(IsCorrectExtension)
        .Select(Path.GetFileNameWithoutExtension)
        .Distinct()
        .Where(IsCorrectName)
        .Select(Assembly.Load!)
        .ToArray();

    private static bool IsCorrectExtension(string fileName)
    {
        return extensionTemplates.Any(s => fileName.EndsWith(s, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsCorrectName(string? path)
    {
        var fileName = Path.GetFileName(path);
        return fileName is not null && fileName.StartsWith(ServiceName, StringComparison.OrdinalIgnoreCase);
    }
}