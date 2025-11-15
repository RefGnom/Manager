using System;
using System.Collections.Generic;
using Manager.Core.Common.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

[IgnoreAutoRegistration]
public interface IIntegrationTestConfigurationBuilder
{
    IIntegrationTestConfigurationBuilder WithAutoRegistration();
    IIntegrationTestConfigurationBuilder WithoutAutoRegistration();
    IIntegrationTestConfigurationBuilder WithNullLogger();
    IIntegrationTestConfigurationBuilder WithRealLogger();
    IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer);
    IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer);
    IIntegrationTestConfigurationBuilder WithDataBase();
    IIntegrationTestConfigurationBuilder WithLocalServer(Dictionary<string, string>? envVariables = null);
    IntegrationTestConfiguration Build();
}