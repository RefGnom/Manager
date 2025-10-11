using System;
using Manager.Core.IntegrationTestsCore.Configuration.Containers;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public record IntegrationTestConfiguration(
    IServiceProvider ServiceProvider,
    ContainerConfiguration ContainerConfiguration
);