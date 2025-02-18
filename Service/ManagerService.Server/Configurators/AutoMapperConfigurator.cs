using AutoMapper;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;
using Microsoft.Extensions.DependencyInjection;

namespace ManagerService.Server.Configurators;

public static class AutoMapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configurator = new MapperConfiguration(
            configure =>
            {
                configure.CreateMap<TimerDto, TimerDbo>().ReverseMap();
                configure.CreateMap<TimerSessionDto, TimerSessionDbo>().ReverseMap();
            }
        );
        var mapper = configurator.CreateMapper();
        services.AddSingleton(mapper);
    }
}