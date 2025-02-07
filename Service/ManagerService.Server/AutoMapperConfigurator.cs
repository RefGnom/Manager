using AutoMapper;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server;

public static class AutoMapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configurator = new MapperConfiguration(
            configure => configure.CreateMap<TimerDto, TimerDbo>()
        );
        var mapper = configurator.CreateMapper();
        services.AddSingleton(mapper);
    }
}