namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public enum ConfigurationActionType
{
    WithDataBase,
    WithLocalServer,
    WithAutoRegistration,
    WithoutAutoRegistration,
    WithNullLogger,
    WithRealLogger,
    CustomizeConfiguration,
    CustomizeServiceCollection,
}