using DotNet.Testcontainers.Containers;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

public record ContainerWithType(
    IContainer Container,
    ContainerType Type
);