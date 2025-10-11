namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public interface IConfigurationAction
{
    ConfigurationActionType Type { get; }
    ConfigurationActionType[] ExcludedTypes { get; }
    void Invoke(ConfigurationActionContext context);
}