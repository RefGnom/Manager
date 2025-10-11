using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Networks;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public record ContainerConfiguration(
    INetwork Network,
    ContainerWithType[] Containers
)
{
    public async Task StartAsync(Func<ContainerWithType, Task> onStart)
    {
        await Network.CreateAsync();
        foreach (var container in Containers)
        {
            await container.Container.StartAsync();
            await onStart(container);
        }
    }

    public async Task DisposeAsync()
    {
        await Network.DisposeAsync();
        foreach (var container in Containers)
        {
            await container.Container.DisposeAsync();
        }
    }
}