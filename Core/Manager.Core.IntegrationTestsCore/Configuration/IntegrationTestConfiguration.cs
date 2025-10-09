using System;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class IntegrationTestConfiguration(
    IServiceProvider serviceProvider
)
{
    public IServiceProvider ServiceProvider { get; set; } = serviceProvider;
}