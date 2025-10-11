using System;
using System.Reflection;
using Manager.Core.Common.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

[IgnoreAutoRegistration]
public interface IIntegrationTestConfigurationBuilder
{
    IIntegrationTestConfigurationBuilder WithTargetTestingAssembly(Assembly targetTestingAssembly);
    IIntegrationTestConfigurationBuilder WithAutoRegistration();
    IIntegrationTestConfigurationBuilder WithoutAutoRegistration();
    IIntegrationTestConfigurationBuilder WithNullLogger();
    IIntegrationTestConfigurationBuilder WithRealLogger();
    IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer);
    IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer);
    IIntegrationTestConfigurationBuilder WithDataBase();
    IIntegrationTestConfigurationBuilder WithLocalServer();
    IntegrationTestConfiguration Build();
}