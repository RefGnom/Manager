using System;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Testcontainers.Networks;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

public record ContainerConfiguration(
    INetwork Network,
    ContainerWithType[] Containers
)
{
    public async Task StartAsync(Func<ContainerWithType, Task> onStart)
    {
        if (Containers.Length == 0)
        {
            return;
        }

        await Network.CreateAsync();
        using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

        foreach (var container in Containers)
        {
            await container.Container.StartAsync(cts.Token);
            await onStart(container);
        }
    }

    public async Task DisposeAsync()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await Network.DeleteAsync(cts.Token).ConfigureAwait(false);
        foreach (var container in Containers)
        {
            await container.Container.DisposeAsync();
        }
    }
}