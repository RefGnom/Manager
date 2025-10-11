namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithoutAutoRegistrationAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithoutAutoRegistration;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithAutoRegistration];

    public void Invoke(ConfigurationActionContext context) { }
}