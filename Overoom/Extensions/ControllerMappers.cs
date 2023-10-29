using Overoom.WEB.Mappers;
using Overoom.WEB.Mappers.Abstractions;

namespace Overoom.Extensions;

public static class ControllerMappers
{
    public static void AddControllerMappers(this IServiceCollection services)
    {
        services.AddSingleton<IFilmsMapper, FilmsMapper>();
        services.AddSingleton<IPlaylistsMapper, PlaylistsMapper>();
        services.AddSingleton<IFilmMapper, FilmMapper>();
        services.AddSingleton<IHomeMapper, HomeMapper>();
        services.AddSingleton<IFilmRoomMapper, FilmRoomMapper>();
        services.AddSingleton<IYoutubeRoomMapper, YoutubeRoomMapper>();
        services.AddSingleton<IFilmManagementMapper, FilmManagementMapper>();
        services.AddSingleton<IProfileMapper, ProfileMapper>();
    }
}