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
using Overoom.Application.Services.FilmsInformation;
using Overoom.Application.Services.FilmsManagement;
using Overoom.Application.Services.Movie;
using Overoom.Application.Services.Playlists;
using Overoom.Application.Services.PlaylistsManagement;
using Overoom.Application.Services.Profile;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;

namespace Overoom.Extensions;

public static class ApplicationMappers
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<ICommentMapper, CommentMapper>();
        services.AddScoped<IFilmKpMapper, FilmKpMapper>();
        services.AddScoped<IFilmManagementMapper, FilmManagementMapper>();
        services.AddScoped<IFilmMapper, FilmMapper>();
        services.AddScoped<IProfileMapper, ProfileMapper>();
        services.AddScoped<IPlaylistsMapper, PlaylistsMapper>();
        services.AddScoped<IFilmsMapper, FilmsMapper>();
        services.AddScoped<IFilmRoomMapper, FilmRoomMapper>();
        services.AddScoped<IYoutubeRoomMapper, YoutubeRoomMapper>();
        services.AddScoped<IStartPageMapper, StartPageMapper>();
        services.AddSingleton<IPlaylistManagementMapper, PlaylistManagementMapper>();
    }
}