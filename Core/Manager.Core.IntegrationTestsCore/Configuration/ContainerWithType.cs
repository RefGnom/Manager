using DotNet.Testcontainers.Containers;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public record ContainerWithType(
    IContainer Container,
    ContainerType Type
);