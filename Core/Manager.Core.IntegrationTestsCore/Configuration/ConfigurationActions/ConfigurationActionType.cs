namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public enum ConfigurationActionType
{
    WithDataBase,
    WithoutDataBase,
    WithLocalServer,
    WithAutoRegistration,
    WithoutAutoRegistration,
    WithNullLogger,
    WithRealLogger,
    CustomizeConfiguration,
    CustomizeServiceCollection,
}