using AutoMapper;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server;

public static class AutoMapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configurator = new MapperConfiguration(
            configure =>
            {
                configure.CreateMap<TimerDto, TimerDbo>();
                configure.CreateMap<TimerRequest, TimerDto>()
                    .ForMember(dest => dest.Id, opt =>
                        opt.MapFrom(src => Guid.NewGuid()))
                    .ForMember(dest => dest.UserId, opt =>
                        opt.MapFrom(src => src.User.Id))
                    .ForMember(dest => dest.StartTime, opt =>
                        opt.MapFrom(src => DateTime.UtcNow))
                    .ForMember(dest => dest.PingTimeout, opt =>
                        opt.MapFrom(src => (TimeSpan?)null))
                    .ForMember(dest => dest.Status, opt =>
                        opt.MapFrom(src => TimerStatus.Started));
            });
        var mapper = configurator.CreateMapper();
        services.AddSingleton(mapper);
    }
}