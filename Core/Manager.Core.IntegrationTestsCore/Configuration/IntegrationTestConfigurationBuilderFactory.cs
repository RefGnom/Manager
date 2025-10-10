namespace Manager.Core.IntegrationTestsCore.Configuration;

public static class IntegrationTestConfigurationBuilderFactory
{
    public static IIntegrationTestConfigurationBuilder Create() =>
        new IntegrationTestConfigurationBuilder(new TestContainerBuilder());
}