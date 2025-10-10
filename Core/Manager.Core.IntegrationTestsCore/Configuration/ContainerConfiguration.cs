using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public record ContainerConfiguration(
    INetwork Network,
    IContainer[] Containers
)
{
    public async Task StartAsync()
    {
        await Network.CreateAsync();
        foreach (var container in Containers)
        {
            await container.StartAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await Network.DisposeAsync();
        foreach (var container in Containers)
        {
            await container.DisposeAsync();
        }
    }
}