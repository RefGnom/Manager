using System.Reflection;
using System.Threading.Tasks;
using Manager.Core.EFCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Manager.Core.IntegrationTestsCore.Configuration.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.Core.IntegrationTestsCore;

[SetUpFixture]
public abstract class SetupFixtureBase
{
    public static IntegrationTestConfiguration TestConfiguration { get; private set; } = null!;

    protected abstract Assembly TargetTestingAssembly { get; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var integrationTestConfigurationBuilder = IntegrationTestConfigurationBuilderFactory.Create()
            .CustomizeConfigurationManager(CustomizeConfiguration)
            .CustomizeServiceCollection(CustomizeServiceCollection)
            .WithTargetTestingAssembly(TargetTestingAssembly)
            .WithAutoRegistration()
            .WithNullLogger()
            .WithDataBase();
        CustomizeConfigurationBuilder(integrationTestConfigurationBuilder);

        TestConfiguration = integrationTestConfigurationBuilder.Build();
        await TestConfiguration.ContainerConfiguration.StartAsync(OnContainerStart);

        await InnerOneTimeSetup();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await TestConfiguration.ContainerConfiguration.DisposeAsync();
    }

    protected virtual void CustomizeConfiguration(IConfigurationManager configurationManager) { }
    protected virtual void CustomizeServiceCollection(IServiceCollection serviceCollection) { }
    protected virtual void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder) { }

    protected virtual async Task OnContainerStart(ContainerWithType containerWithType)
    {
        if (containerWithType.Type != ContainerType.DataBase)
        {
            return;
        }

        var dbContextWrapperFactory = TestConfiguration.ServiceProvider.GetService<IDbContextWrapperFactory>();
        if (dbContextWrapperFactory != null)
        {
            var dbContextWrapper = dbContextWrapperFactory.Create();
            await dbContextWrapper.Database.EnsureCreatedAsync();
        }
    }

    protected virtual Task InnerOneTimeSetup() => Task.CompletedTask;
}