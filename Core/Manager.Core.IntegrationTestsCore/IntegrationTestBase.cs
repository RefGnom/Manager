using System;
using AutoFixture;
using Manager.Core.EFCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.Core.IntegrationTestsCore;

[TestFixture]
[Category("IntegrationTests")]
public abstract class IntegrationTestBase
{
    /// <summary>
    ///     Fixture для создания полностью заполненных моделей случайными значениями
    /// </summary>
    protected readonly Fixture Fixture = new();

    /// <summary>
    ///     Сконфигурированный ServiceProvider, по умолчанию предоставляет все
    ///     реализации тестируемой сборкии тестовой сборки
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = SetupFixtureBase.TestConfiguration.ServiceProvider;

    /// <summary>
    ///     Контекст для тестов. Можно работать с ним не используя тестируемый сервис.
    ///     Null если база данных не сконфигурирована
    /// </summary>
    protected IDataContext DataContext { get; } =
        SetupFixtureBase.TestConfiguration.ServiceProvider.GetService<IDataContext>()!;

    /// <summary>
    ///     Распределённый кэш для тестов. Можно работать с ним не используя тестируемый сервис.
    ///     Null если распределённый кэш не сконфигурирован
    /// </summary>
    protected IDistributedCache DistributedCache { get; } =
        SetupFixtureBase.TestConfiguration.ServiceProvider.GetService<IDistributedCache>()!;
}