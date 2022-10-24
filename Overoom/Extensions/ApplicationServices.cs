using Overoom.Application.Abstractions.Interfaces.Comments;
using Overoom.Application.Abstractions.Interfaces.Films;
using Overoom.Application.Abstractions.Interfaces.Playlists;
using Overoom.Application.Abstractions.Interfaces.Rooms;
using Overoom.Application.Abstractions.Interfaces.StartPage;
using Overoom.Application.Abstractions.Interfaces.Users;
using Overoom.Application.Services.Services;
using Overoom.Application.Services.Services.Comments;
using Overoom.Application.Services.Services.Films;
using Overoom.Application.Services.Services.Playlists;
using Overoom.Application.Services.Services.Rooms;
using Overoom.Application.Services.Services.StartPage;
using Overoom.Application.Services.Services.Users;

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
        services.AddScoped<IRoomDeleterManager, RoomDeleterManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IPlaylistManager, PlaylistManager>();
        services.AddScoped<IFilmLoaderService, FilmLoaderService>();
        services.AddScoped<IStartPageService, StartPageService>();
    }
}