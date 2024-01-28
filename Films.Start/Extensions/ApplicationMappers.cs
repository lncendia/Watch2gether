using Films.Start.Application.Abstractions.Comments.Interfaces;
using Films.Start.Application.Abstractions.Films.Interfaces;
using Films.Start.Application.Abstractions.FilmsInformation.Interfaces;
using Films.Start.Application.Abstractions.FilmsManagement.Interfaces;
using Films.Start.Application.Abstractions.Movie.Interfaces;
using Films.Start.Application.Abstractions.Playlists.Interfaces;
using Films.Start.Application.Abstractions.PlaylistsManagement.Interfaces;
using Films.Start.Application.Abstractions.Profile.Interfaces;
using Films.Start.Application.Abstractions.Rooms.Interfaces;
using Films.Start.Application.Abstractions.StartPage.Interfaces;
using Films.Start.Application.Services.Comments;
using Films.Start.Application.Services.Films;
using Films.Start.Application.Services.FilmsCatalog;
using Films.Start.Application.Services.FilmsInformation;
using Films.Start.Application.Services.FilmsManagement;
using Films.Start.Application.Services.PlaylistsCatalog;
using Films.Start.Application.Services.PlaylistsManagement;
using Films.Start.Application.Services.Profile;
using Films.Start.Application.Services.Rooms;
using Films.Start.Application.Services.StartPage;

namespace Films.Start.Extensions;

public static class ApplicationMappers
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddSingleton<ICommentMapper, CommentMapper>();
        services.AddSingleton<IFilmKpMapper, FilmKpMapper>();
        services.AddSingleton<IFilmManagementMapper, FilmManagementMapper>();
        services.AddSingleton<IFilmMapper, FilmMapper>();
        services.AddSingleton<IProfileMapper, ProfileMapper>();
        services.AddSingleton<IPlaylistsMapper, PlaylistsMapper>();
        services.AddSingleton<IFilmsMapper, FilmsMapper>();
        services.AddSingleton<IFilmRoomMapper, FilmRoomMapper>();
        services.AddSingleton<IYoutubeRoomMapper, YoutubeRoomMapper>();
        services.AddSingleton<IStartPageMapper, StartPageMapper>();
        services.AddSingleton<IPlaylistManagementMapper, PlaylistManagementMapper>();
    }
}