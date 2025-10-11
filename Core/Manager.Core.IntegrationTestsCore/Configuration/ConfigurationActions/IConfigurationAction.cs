using Manager.Core.Common.DependencyInjection.Attributes;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

[IgnoreAutoRegistration]
public interface IConfigurationAction
{
    ConfigurationActionType Type { get; }
    ConfigurationActionType[] ExcludedTypes { get; }
    void Invoke(ConfigurationActionContext context);
}