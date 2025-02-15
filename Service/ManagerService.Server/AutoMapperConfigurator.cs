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
                configure.CreateMap<TimerDto, TimerDbo>().ReverseMap();
                configure.CreateMap<TimerSessionDto, TimerSessionDbo>().ReverseMap();
                configure.CreateMap<StartTimerRequest, TimerDto>()
                    .ForMember(
                        dest => dest.Id,
                        opt =>
                            opt.MapFrom(src => Guid.NewGuid()))
                    .ForMember(
                        dest => dest.UserId,
                        opt =>
                            opt.MapFrom(src => src.User.Id)
                    )
                    .ForMember(
                        dest => dest.Status,
                        opt =>
                            opt.MapFrom(src => TimerStatus.Started)
                    );
                configure.CreateMap<TimerDto, TimerResponse>()
                    .ForMember(
                        dest => dest.ElapsedTime,
                        opt =>
                            opt.MapFrom(src => DateTime.UtcNow - src.StartTime))
                    .ForMember(
                        dest => dest.Sessions,
                        opt =>
                            opt.MapFrom(src => Array.Empty<TimerSessionResponse>())
                    );
            }
        );
        var mapper = configurator.CreateMapper();
        services.AddSingleton(mapper);
    }
}