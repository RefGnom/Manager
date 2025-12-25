using System.Reflection;
using Manager.Core.Common.DependencyInjection.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрирует в DI класс настроек с валидацией DataAnnotations при старте приложения
    /// </summary>
    public static IServiceCollection ConfigureOptionsWithValidation<TOptions>(this IServiceCollection serviceCollection)
        where TOptions : class
    {
        var optionsType = typeof(TOptions);
        var attributePath = optionsType.GetCustomAttribute<OptionPathAttribute>()?.Path;

        serviceCollection.AddOptions<TOptions>()
            .BindConfiguration(attributePath ?? optionsType.Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return serviceCollection;
    }
}