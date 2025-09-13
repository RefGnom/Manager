using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration;

public static class ServiceCollectionExtensions
{
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