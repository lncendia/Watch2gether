using Films.Application.Abstractions.MovieApi.Interfaces;
using Films.Infrastructure.Movie.Abstractions;
using Films.Infrastructure.Movie.Services;

namespace Films.Start.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var kpToken = configuration.GetRequiredValue<string>("MovieApi:Kinopoisk");
        var videoCdnToken = configuration.GetRequiredValue<string>("MovieApi:VideoCdn");
        
        services.AddSingleton<IKpResponseParser, KpResponseParser>();
        services.AddSingleton<IVideoCdnResponseParser, VideoCdnResponseParser>();
        
        services.AddSingleton<IKpApiService, KpApi>(s => new KpApi(kpToken, s.GetRequiredService<IKpResponseParser>()));
        
        services.AddSingleton<IVideoCdnApiService, VideoCdnApi>(s =>
            new VideoCdnApi(videoCdnToken, s.GetRequiredService<IVideoCdnResponseParser>()));
    }
}