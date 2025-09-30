using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует в DI класс настроек с валидацией DataAnnotations при старте приложения
    /// </summary>
    public static IServiceCollection ConfigureOptionsWithValidation<TOptions>(this IServiceCollection serviceCollection)
        where TOptions : class
    {
        serviceCollection.AddOptions<TOptions>()
            .BindConfiguration(typeof(TOptions).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}