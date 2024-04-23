using Films.Infrastructure.Load.HostedServices;
using Films.Infrastructure.Load.Kinopoisk;
using Films.Infrastructure.Load.Kodik;

namespace Films.Start.Extensions;

public static class LoaderServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var kpToken = configuration.GetRequiredValue<string>("MovieApi:Kinopoisk");
        var kodikToken = configuration.GetRequiredValue<string>("MovieApi:Kodik");
        
        services.AddSingleton<IKinopoiskApi, KinopoiskApi>(_ => new KinopoiskApi(kpToken));
        
        services.AddSingleton<IKodikApi, KodikApi>(_ => new KodikApi(kodikToken));

        // services.AddHostedService<FilmLoadHostedService>();
    }
}