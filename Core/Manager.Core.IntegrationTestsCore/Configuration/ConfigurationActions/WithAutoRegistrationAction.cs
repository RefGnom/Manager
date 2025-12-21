using System;
using System.IO;
using System.Reflection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithAutoRegistrationAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithAutoRegistration;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithoutAutoRegistration];

    public void Invoke(ConfigurationActionContext context)
    {
        context.ServiceCollection.UseAutoRegistrationForAssembly(context.TargetAssembly)
            .UseAutoRegistrationForAssembly(GetTestsAssembly())
            .UseAutoRegistrationForCoreCommon()
            .UseAutoRegistrationForCoreNetworking();
    }

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        if (!testAssemblyName.StartsWith("Manager."))
        {
            testAssemblyName = $"Manager.{testAssemblyName}";
        }

        return Assembly.Load(testAssemblyName);
    }
}