using System;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class IntegrationTestConfiguration(
    IServiceProvider serviceProvider,
    ContainerConfiguration containerConfiguration
)
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
    public ContainerConfiguration ContainerConfiguration { get; } = containerConfiguration;
}