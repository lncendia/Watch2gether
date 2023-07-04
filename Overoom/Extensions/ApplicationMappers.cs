using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Catalog.Interfaces;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Films.Catalog;
using Overoom.Application.Services.Films.Load;
using Overoom.Application.Services.Playlists;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;

namespace Overoom.Extensions;

public static class ApplicationMappers
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<ICommentMapper, CommentMapper>();
        services.AddScoped<IFilmKpMapper, FilmKpMapper>();
        services.AddScoped<IFilmMapper, FilmMapper>();
        services.AddScoped<IPlaylistMapper, PlaylistMapper>();
        services.AddScoped<IFilmRoomMapper, FilmRoomMapper>();
        services.AddScoped<IYoutubeRoomMapper, YoutubeRoomMapper>();
        services.AddScoped<IStartPageMapper, StartPageMapper>();
    }
}