using Overoom.WEB.Mappers;
using Overoom.WEB.Mappers.Abstractions;

namespace Overoom.Extensions;

public static class ControllerMappers
{
    public static void AddControllerMappers(this IServiceCollection services)
    {
        services.AddScoped<IFilmMapper, FilmMapper>();
        services.AddScoped<IHomeMapper, HomeMapper>();
        services.AddScoped<IFilmRoomMapper, FilmRoomMapper>();
        services.AddScoped<IYoutubeRoomMapper, YoutubeRoomMapper>();
        services.AddScoped<IFilmLoadMapper, FilmLoadMapper>();
    }
}