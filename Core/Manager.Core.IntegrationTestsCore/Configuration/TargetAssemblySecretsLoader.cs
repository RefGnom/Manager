using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public static class TargetAssemblySecretsLoader
{
    private const string EnvironmentFileName = ".env";

    public static IReadOnlyDictionary<string, string> LoadAsDictionary(Assembly targetAssembly)
    {
        var assemblyName = Path.GetFileNameWithoutExtension(targetAssembly.ManifestModule.Name);
        var pathPrefix = Path.Combine(Enumerable.Repeat("..", 4).ToArray());
        var environmentFilePath = Path.Combine(pathPrefix, assemblyName, EnvironmentFileName);
        if (File.Exists(environmentFilePath))
        {
            return LoadSecretsByPath(environmentFilePath);
        }

        // Удаляем префикс Manager.
        var environmentFilePathWithoutPrefix = Path.Combine(pathPrefix, assemblyName.Remove(0, 8), EnvironmentFileName);
        return File.Exists(environmentFilePathWithoutPrefix)
            ? LoadSecretsByPath(environmentFilePathWithoutPrefix)
            : new Dictionary<string, string>();
    }

    private static Dictionary<string, string> LoadSecretsByPath(string environmentFilePath)
    {
        return File.ReadAllLines(environmentFilePath)
            .Select(x => x.Split('='))
            .ToDictionary(x => x[0], x => x[1]);
    }
}