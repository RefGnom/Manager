namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithoutPostgresDataBaseAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithoutDataBase;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithDataBase];
    public void Invoke(ConfigurationActionContext context) { }
}