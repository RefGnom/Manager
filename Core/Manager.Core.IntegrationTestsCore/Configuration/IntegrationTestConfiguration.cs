using System;
using DotNet.Testcontainers.Containers;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class IntegrationTestConfiguration(
    IServiceProvider serviceProvider,
    IContainer? serverContainer
)
{
    public IServiceProvider ServiceProvider { get; set; } = serviceProvider;
    public IContainer? ServerContainer { get; } = serverContainer;
}