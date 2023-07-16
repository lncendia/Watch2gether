using Overoom.Application.Abstractions.Comments.Interfaces;
using Overoom.Application.Abstractions.Content.Interfaces;
using Overoom.Application.Abstractions.FilmsManagement.Interfaces;
using Overoom.Application.Abstractions.Movie.Interfaces;
using Overoom.Application.Abstractions.Rooms.Interfaces;
using Overoom.Application.Abstractions.StartPage.Interfaces;
using Overoom.Application.Abstractions.Users.Interfaces;
using Overoom.Application.Services.Comments;
using Overoom.Application.Services.Content;
using Overoom.Application.Services.FilmsManagement;
using Overoom.Application.Services.Movie;
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
        services.AddScoped<IFilmManagementService, FilmManagementService>();
        services.AddScoped<IContentManager, ContentManager>();
        services.AddScoped<IFilmRoomManager, FilmRoomManager>();
        services.AddScoped<IYoutubeRoomManager, YoutubeRoomManager>();
        services.AddScoped<IStartPageService, StartPageService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IFilmInfoService, FilmInfoService>();
        services.AddMemoryCache();
    }
}