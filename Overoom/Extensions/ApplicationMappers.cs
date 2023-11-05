using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Interfaces;
using Overoom.Application.Abstractions.FilmsInformation.Interfaces;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Application.Abstractions.PlaylistsManagement.Interfaces;
using Overoom.Application.Abstractions.Profile.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Films;
using Overoom.Application.Services.FilmsCatalog;
using Overoom.Application.Services.FilmsInformation;
using Overoom.Application.Services.FilmsManagement;
using Overoom.Application.Services.PlaylistsCatalog;
using Overoom.Application.Services.PlaylistsManagement;
using Overoom.Application.Services.Profile;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;

namespace Overoom.Extensions;

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