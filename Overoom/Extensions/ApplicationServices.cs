using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Films.Catalog.Interfaces;
using Overoom.Application.Abstractions.Films.Load.Interfaces;
using Overoom.Application.Abstractions.Playlists.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Films.Catalog;
using Overoom.Application.Services.Films.Load;
using Overoom.Application.Services.Playlists;
using Overoom.Application.Services.Rooms;
using Overoom.Application.Services.StartPage;
using Overoom.Application.Services.Users;

namespace Overoom.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommentManager, CommentManager>();
        services.AddScoped<IFilmManager, FilmManager>();
        services.AddScoped<IFilmLoadService, FilmLoadService>();
        services.AddScoped<IPlaylistManager, PlaylistManager>();
        services.AddScoped<IFilmRoomManager, FilmRoomManager>();
        services.AddScoped<IYoutubeRoomManager, YoutubeRoomManager>();
        services.AddScoped<IStartPageService, StartPageService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IUserProfileService, UserProfileService>();
    }
}