using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Catalog.Interfaces;
using Overoom.Application.Abstractions.Films.Playlist.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Film;
using Overoom.Application.Services.Films.Catalog;
using Overoom.Application.Services.Films.Playlists;
using Overoom.Application.Services.Kinopoisk;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;

namespace Overoom.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserParametersService, UserParametersService>();
        services.AddScoped<IUserSecurityService, UserSecurityService>();
        services.AddScoped<IFilmRoomManager, FilmRoomManager>();
        services.AddScoped<IYoutubeRoomManager, YoutubeRoomManager>();
        services.AddScoped<ICommentManager, CommentManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IPlaylistManager, PlaylistManager>();
        services.AddScoped<IFilmLoaderService, FilmInfoService>();
        services.AddScoped<IStartPageService, StartPageService>();
    }
}