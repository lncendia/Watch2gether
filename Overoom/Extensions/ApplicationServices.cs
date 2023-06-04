using Overoom.Application.Abstractions.Comment.Interfaces;
using Overoom.Application.Abstractions.Film.Interfaces;
using Overoom.Application.Abstractions.Room.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Abstractions.User.Interfaces;
using Overoom.Application.Services.Comment;
using Overoom.Application.Services.Film;
using Overoom.Application.Services.Room;
using Overoom.Application.Services.Services;
using Overoom.Application.Services.StartPage;
using Overoom.Application.Services.User;

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